using Unity.Netcode;
using UnityEngine;

public class NetworkManagerPersist : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}