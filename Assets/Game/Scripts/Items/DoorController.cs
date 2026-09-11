using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class DoorController : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float rotationSpeed = 180f;

    private Rigidbody2D rb;

    private float closedAngle;
    private bool playerNearby;
    private bool isOpen;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        closedAngle = rb.rotation;
    }

    private void Update()
    {
        if (!playerNearby || Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            isOpen = !isOpen;
        }
    }

    private void FixedUpdate()
    {
        float targetAngle;

        if (isOpen)
            targetAngle = closedAngle + openAngle;
        else
            targetAngle = closedAngle;

        float newAngle = Mathf.MoveTowardsAngle(
            rb.rotation,
            targetAngle,
            rotationSpeed * Time.fixedDeltaTime
        );

        rb.MoveRotation(newAngle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}