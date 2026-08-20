using Unity.Netcode;
using UnityEngine;

public class StackHats : NetworkBehaviour
{
     [SerializeField] private Transform hatPosition;

    [SerializeField] private GameObject hatPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
[Rpc(SendTo.Server)]
    public void RequestStackHatsRpc()
    {
        StackHatsOnChamp();
    }
    
    public void StackHatsOnChamp()
    {
        if (!IsServer) return;

        var numberOfHats = GetComponent<Points>().points.Value;

        for(int i = 0; i < numberOfHats; i++)
        {
            GameObject spawnedHat = Instantiate(hatPrefab, hatPosition.position, Quaternion.identity);

            NetworkObject networkObject = spawnedHat.GetComponent<NetworkObject>();

            networkObject.Spawn();

            if (networkObject.TrySetParent(hatPosition, false))
            {
                spawnedHat.transform.localPosition = new Vector3(0f, i, 0f);
            }
        }
    }
}


