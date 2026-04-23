using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.AI;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;

    private Vector2 moveInput;

    public float speed = 10f;
    public float jumpForce = 5f;

    private bool isGrounded = true;
    public float groundCheckDistance = 0.6f;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    public GameObject pickupPrefab;

    public float minDistance = 2f;
    public float mapHalfSize = 15f;

    private List<Vector3> usedPositions = new List<Vector3>();

    public static bool gameStarted = false;
    public static bool playerDead = false;

    void Start()
    {
        gameStarted = false;
        playerDead = false;

        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);

        RandomizeAllPickups();
    }


    public void OnMove(InputValue value)
    {
        if (playerDead) return;

        moveInput = value.Get<Vector2>();

        if (!gameStarted && moveInput.sqrMagnitude > 0.01f)
        {
            gameStarted = true;
            GameTimer.instance.Begin();
        }
    }

    public void OnJump(InputValue value)
    {
        if (!gameStarted || playerDead) return;

        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        CheckWinCondition();
    }

    void FixedUpdate()
    {
        if (playerDead) return;

        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!gameStarted || playerDead) return;

        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

    
            SpawnNewPickup();
            SpawnNewPickup();
        }
    }

    void CheckWinCondition()
    {
        GameObject[] pickupsLeft = GameObject.FindGameObjectsWithTag("PickUp");

        if (pickupsLeft.Length == 0)
        {
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Win!";

            GameTimer.instance.Stop(true);

            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null)
                Destroy(enemy);
        }
    }

    void RandomizeAllPickups()
    {
        usedPositions.Clear();

        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");

        foreach (GameObject p in pickups)
        {
            Vector3 pos = GetValidNavMeshPoint();
            p.transform.position = pos;
            usedPositions.Add(pos);
        }
    }

    Vector3 GetRandomPointOnNavMesh()
    {
        NavMeshTriangulation mesh = NavMesh.CalculateTriangulation();

        int index = Random.Range(0, mesh.indices.Length / 3) * 3;

        Vector3 v1 = mesh.vertices[mesh.indices[index]];
        Vector3 v2 = mesh.vertices[mesh.indices[index + 1]];
        Vector3 v3 = mesh.vertices[mesh.indices[index + 2]];

        Vector3 randomPoint = v1 +
            Random.Range(0f, 1f) * (v2 - v1) +
            Random.Range(0f, 1f) * (v3 - v1);

        randomPoint.y += 0.5f;

        return randomPoint;
    }


    Vector3 GetValidNavMeshPoint()
    {
        for (int i = 0; i < 50; i++)
        {
            Vector3 point = GetRandomPointOnNavMesh();

            if (Mathf.Abs(point.x) > mapHalfSize || Mathf.Abs(point.z) > mapHalfSize)
                continue;

            bool tooClose = false;
            foreach (Vector3 used in usedPositions)
            {
                if (Vector3.Distance(point, used) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return point;
        }

        return new Vector3(0, 1, 0);
    }


    void SpawnNewPickup()
    {
        Vector3 spawnPos = GetValidNavMeshPoint();
        usedPositions.Add(spawnPos);
        Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
    }

    void SetCountText()
    {
        countText.text = "Count: " + count;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerDead = true;
            gameStarted = false;

            GameTimer.instance.Stop(false);

            gameObject.SetActive(false);

            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}
