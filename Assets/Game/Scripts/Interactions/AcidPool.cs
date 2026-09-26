using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AcidPool : MonoBehaviour
{
    [SerializeField, Min(0f)] private float damagePerSecond = 25f;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerHealth player))
            player.TakeDamage(damagePerSecond * Time.fixedDeltaTime);
    }
}
