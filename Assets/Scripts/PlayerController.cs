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
        }
    }

    void CheckWinCondition()
    {
        GameObject[] pickupsLeft = GameObject.FindGameObjectsWithTag("PickUp");

        if (pickupsLeft.Length == 0)
        {
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Win!";

            GameTimer.instance.Stop();

            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null)
                Destroy(enemy);
        }
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

            GameTimer.instance.Stop();

            gameObject.SetActive(false);

            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}
