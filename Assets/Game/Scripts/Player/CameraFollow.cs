using Unity.Netcode;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f;

    private Transform target;

    private void LateUpdate()
    {
        if (target == null)
        {
            FindLocalPlayer();
            return;
        }

        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    private void FindLocalPlayer()
    {
        if (NetworkManager.Singleton == null)
            return;

        foreach (var networkObject in FindObjectsOfType<NetworkObject>())
        {
            if (!networkObject.IsOwner)
                continue;

            if (networkObject.CompareTag("Player"))
            {
                target = networkObject.transform;
                return;
            }
        }
    }
}