using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Multiplayer : MonoBehaviour
{
    private bool sceneLoaded = false;

    public void StartHostButtonClick()
    {
        NetworkManager.Singleton.NetworkConfig.EnableSceneManagement = false;

        bool started = NetworkManager.Singleton.StartHost();

        if (started)
        {
            Debug.Log("Host started!");

            LoadCharacterSelect();
        }
        else
        {
            Debug.LogError("Failed to start Host!");
        }
    }

    public void StartClientButtonClick()
    {
        NetworkManager.Singleton.NetworkConfig.EnableSceneManagement = false;

        bool started = NetworkManager.Singleton.StartClient();

        if (started)
        {
            Debug.Log("Client is connecting...");

            StartCoroutine(WaitForConnection());
        }
        else
        {
            Debug.LogError("Failed to start Client!");
        }
    }

    private IEnumerator WaitForConnection()
    {
        float timeout = 10f;
        float timer = 0f;

        while (!NetworkManager.Singleton.IsConnectedClient && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (NetworkManager.Singleton.IsConnectedClient)
        {
            Debug.Log("Client connected!");

            LoadCharacterSelect();
        }
        else
        {
            Debug.LogError("Host not found!");
        }
    }

    private void LoadCharacterSelect()
    {
        sceneLoaded = true;

        Debug.Log("Loading CharacterSelect locally...");

        SceneManager.LoadScene("CharacterSelect");
    }
}