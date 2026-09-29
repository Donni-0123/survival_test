using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Не знищувати персонажа при переході між сценами
        DontDestroyOnLoad(gameObject);

        // Одразу встановлюємо правильну видимість
        UpdateCharacterVisibility();

        Debug.Log(
            $"PLAYER SPAWNED: " +
            $"ObjectId={NetworkObjectId}, " +
            $"Owner={OwnerClientId}, " +
            $"IsOwner={IsOwner}, " +
            $"IsServer={IsServer}, " +
            $"IsClient={IsClient}"
        );
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode)
    {
        UpdateCharacterVisibility();
    }

    private void Update()
    {
        // Персонажі не рухаються поза Testova
        if (SceneManager.GetActiveScene().name != "Testova")
        {
            movement = Vector2.zero;
            return;
        }

        // Кожен клієнт керує тільки своїм персонажем
        if (!IsOwner)
        {
            movement = Vector2.zero;
            return;
        }

        movement = Vector2.zero;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.wKey.isPressed)
            movement.y += 1;

        if (Keyboard.current.sKey.isPressed)
            movement.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            movement.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            movement.x += 1;

        movement = movement.normalized;
    }

    private void FixedUpdate()
    {
        // Поза Testova персонаж не рухається
        if (SceneManager.GetActiveScene().name != "Testova")
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Чужий персонаж не керується локальним клієнтом
        if (!IsOwner)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movement * moveSpeed;
    }

    private void UpdateCharacterVisibility()
    {
        bool isTestova =
            SceneManager.GetActiveScene().name == "Testova";

        // Вмикаємо/вимикаємо Renderer
        Renderer[] renderers =
            GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = isTestova;
        }

        // Вмикаємо/вимикаємо Collider
        Collider2D[] colliders =
            GetComponentsInChildren<Collider2D>(true);

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = isTestova;
        }
    }
}