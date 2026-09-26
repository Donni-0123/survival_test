using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Min(1f)] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    private Rigidbody2D rb;
    private Vector2 respawnPosition;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        respawnPosition = rb.position;
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        if (currentHealth <= 0f)
            Respawn();
    }

    public void SetRespawnPoint(Vector2 position)
    {
        respawnPosition = position;
    }

    private void Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = respawnPosition;
        currentHealth = maxHealth;
    }
}
