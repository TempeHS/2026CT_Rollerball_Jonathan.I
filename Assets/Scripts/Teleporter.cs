using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform destination;   // Where this teleporter sends the player
    public float cooldownTime = 0.5f;

    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTeleport)
        {
            // Move player to destination
            other.transform.position = destination.position;

            // Prevent instant re-teleporting
            Teleporter destTeleporter = destination.GetComponent<Teleporter>();
            if (destTeleporter != null)
            {
                destTeleporter.DisableTeleportFor(cooldownTime);
            }

            canTeleport = false;
            Invoke(nameof(ResetTeleport), cooldownTime);
        }
    }

    public void DisableTeleportFor(float time)
    {
        canTeleport = false;
        Invoke(nameof(ResetTeleport), time);
    }

    private void ResetTeleport()
    {
        canTeleport = true;
    }
}
