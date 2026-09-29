using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionNetworkManager : NetworkBehaviour
{
    private bool sceneLoading = false;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InvokeRepeating(nameof(CheckHostReady), 1f, 0.5f);
        }
    }

    private void CheckHostReady()
    {
        if (!IsServer || sceneLoading)
            return;

        // Перевіряємо, чи є Host PlayerObject
        if (NetworkManager.Singleton.LocalClient == null)
            return;

        if (NetworkManager.Singleton.LocalClient.PlayerObject == null)
            return;

        PlayerSelection selection =
            NetworkManager.Singleton.LocalClient.PlayerObject
                .GetComponent<PlayerSelection>();

        if (selection == null)
            return;

        // Host ще не вибрав персонажа
        if (selection.SelectedCharacter.Value == -1)
            return;

        // Host вибрав персонажа
        sceneLoading = true;

        Debug.Log(
            $"Host selected character: {selection.SelectedCharacter.Value}"
        );

        NetworkManager.Singleton.SceneManager.LoadScene(
            "Testova",
            LoadSceneMode.Single
        );
    }
}