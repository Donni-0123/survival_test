using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkCharacterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject player2Prefab;

    private readonly HashSet<ulong> spawnedClients = new HashSet<ulong>();

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void Update()
    {
        if (NetworkManager.Singleton == null)
            return;

        // Спавн персонажів виконує тільки Host/Server
        if (!NetworkManager.Singleton.IsServer)
            return;

        // Персонажів створюємо тільки в Testova
        if (SceneManager.GetActiveScene().name != "Testova")
            return;

        TrySpawnCharacters();
    }

    private void TrySpawnCharacters()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            ulong clientId = client.ClientId;

            // Цьому клієнту персонаж уже створений
            if (spawnedClients.Contains(clientId))
                continue;

            // Перевіряємо PlayerSelection
            if (client.PlayerObject == null)
                continue;

            PlayerSelection selection =
                client.PlayerObject.GetComponent<PlayerSelection>();

            if (selection == null)
                continue;

            // -1 означає, що персонаж ще не вибраний
            int character = selection.SelectedCharacter.Value;

            if (character == -1)
                continue;

            SpawnCharacter(clientId, character);

            spawnedClients.Add(clientId);
        }
    }

    private void SpawnCharacter(ulong clientId, int character)
    {
        GameObject prefab;

        if (character == 0)
        {
            prefab = playerPrefab;
        }
        else if (character == 1)
        {
            prefab = player2Prefab;
        }
        else
        {
            Debug.LogError(
                $"Невідомий номер персонажа: {character}"
            );

            return;
        }

        if (prefab == null)
        {
            Debug.LogError(
                "Character prefab не призначений у NetworkCharacterSpawner!"
            );

            return;
        }

        Vector3 spawnPosition = GetSpawnPosition(clientId);

        GameObject player = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );

        // Персонаж повинен залишатися між локальними сценами
        DontDestroyOnLoad(player);

        NetworkObject networkObject =
            player.GetComponent<NetworkObject>();

        if (networkObject == null)
        {
            Debug.LogError(
                $"На {prefab.name} немає NetworkObject!"
            );

            Destroy(player);
            return;
        }

        // Створюємо NetworkObject і передаємо ownership
        // конкретному клієнту
        networkObject.SpawnWithOwnership(clientId);

        Debug.Log(
            $"SPAWN: character={character}, " +
            $"client={clientId}, " +
            $"networkObjectId={networkObject.NetworkObjectId}"
        );
    }

    private Vector3 GetSpawnPosition(ulong clientId)
    {
        // Host
        if (clientId == 0)
        {
            return new Vector3(-2f, 0f, 0f);
        }

        // Player
        return new Vector3(2f, 0f, 0f);
    }

    private void OnClientConnected(ulong clientId)
    {
        // Цю логіку виконує тільки Host
        if (NetworkManager.Singleton == null)
            return;

        if (!NetworkManager.Singleton.IsServer)
            return;

        Debug.Log(
            $"Client {clientId} connected. Updating observers."
        );

        // Показуємо новому клієнту вже існуючі
        // NetworkObjects
        foreach (var networkObject in FindObjectsOfType<NetworkObject>())
        {
            if (!networkObject.IsSpawned)
                continue;

            networkObject.NetworkShow(clientId);
        }
    }
}