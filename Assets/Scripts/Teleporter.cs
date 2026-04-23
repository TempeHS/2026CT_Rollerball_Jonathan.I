using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform destination;   
    public float cooldownTime = 0.5f;

    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTeleport)
        {
            
            other.transform.position = destination.position;

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
