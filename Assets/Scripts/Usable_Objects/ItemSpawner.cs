using Unity.Netcode;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        SpawnItem();
    }

    private void SpawnItem()
    {
        GameObject spawnedItem = Instantiate(itemPrefab, transform.position, transform.rotation);
        NetworkObject networkObject = spawnedItem.GetComponent<NetworkObject>();

        if(networkObject != null)
        {
            networkObject.Spawn();
        }
    }
}
