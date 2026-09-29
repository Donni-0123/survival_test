using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientSceneController : MonoBehaviour
{
    private void Start()
    {
        if (NetworkManager.Singleton == null)
            return;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
            return;

        if (clientId != NetworkManager.Singleton.LocalClientId)
            return;

        Debug.Log("ClientSceneController: Client connected!");

        StartCoroutine(ReturnToCharacterSelect());
    }

    private IEnumerator ReturnToCharacterSelect()
    {
        // Даємо NGO завершити підключення
        yield return new WaitForSeconds(1f);

        Debug.Log("ClientSceneController: Loading CharacterSelect...");

        SceneManager.LoadScene("CharacterSelect");
    }
}