using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform player;
    public float height = 1.5f;

    void LateUpdate()
    {
        if (player == null)
            return;

        // Keep arrow above the player
        transform.position = player.position + new Vector3(0, height, 0);

        // Lock rotation so it never spins
        transform.rotation = Quaternion.identity;
    }
}
