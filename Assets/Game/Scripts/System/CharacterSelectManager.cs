using Unity.Netcode;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public void SelectCharacter1()
    {
        PlayerSelection playerSelection = GetLocalPlayerSelection();

        if (playerSelection == null)
        {
            Debug.LogError("Не знайдено PlayerSelection локального гравця!");
            return;
        }

        playerSelection.SelectCharacter1();
    }

    public void SelectCharacter2()
    {
        PlayerSelection playerSelection = GetLocalPlayerSelection();

        if (playerSelection == null)
        {
            Debug.LogError("Не знайдено PlayerSelection локального гравця!");
            return;
        }

        playerSelection.SelectCharacter2();
    }

    private PlayerSelection GetLocalPlayerSelection()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton == null");
            return null;
        }

        if (NetworkManager.Singleton.LocalClient == null)
        {
            Debug.LogError("LocalClient == null");
            return null;
        }

        if (NetworkManager.Singleton.LocalClient.PlayerObject == null)
        {
            Debug.LogError("PlayerObject == null");
            return null;
        }

        PlayerSelection selection =
            NetworkManager.Singleton.LocalClient.PlayerObject
                .GetComponent<PlayerSelection>();

        if (selection == null)
        {
            Debug.LogError("PlayerObject є, але на ньому немає PlayerSelection!");
            return null;
        }

        return selection;
    }
}