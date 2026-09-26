using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RespawnTotem : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerHealth player))
            player.SetRespawnPoint(transform.position);
    }
}
