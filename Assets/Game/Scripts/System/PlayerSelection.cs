using Unity.Netcode;
using UnityEngine;

public class PlayerSelection : NetworkBehaviour
{
    public NetworkVariable<int> SelectedCharacter = new NetworkVariable<int>(
        -1,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SelectCharacter1()
    {
        if (!IsOwner)
            return;

        SelectedCharacter.Value = 0;

        Debug.Log($"Player {OwnerClientId} selected Character 1");

        LoadTestova();
    }

    public void SelectCharacter2()
    {
        if (!IsOwner)
            return;

        SelectedCharacter.Value = 1;

        Debug.Log($"Player {OwnerClientId} selected Character 2");

        LoadTestova();
    }

    private void LoadTestova()
    {
        Debug.Log($"Player {OwnerClientId} loading Testova...");

        UnityEngine.SceneManagement.SceneManager.LoadScene("Testova");
    }
}