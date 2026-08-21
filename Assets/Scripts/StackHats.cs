using Unity.Netcode;
using UnityEngine;

public class StackHats : NetworkBehaviour
{
     [SerializeField] private Transform hatPosition;

    [SerializeField] private GameObject hatPrefab;

    [SerializeField] private float distanceBetweenHatsMultiplier = 0.15f;
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

        float stackHeight = numberOfHats - 1f;

        if(stackHeight < 0) stackHeight = 0;

        GameObject spawnedHat = Instantiate(hatPrefab, hatPosition.position, Quaternion.identity);
        NetworkObject networkObject = spawnedHat.GetComponent<NetworkObject>();
        networkObject.Spawn();

        if (networkObject.TrySetParent(this.transform, false))
        {
            float verticalOffset = stackHeight * distanceBetweenHatsMultiplier;
            spawnedHat.transform.position = hatPosition.position + new Vector3(0,verticalOffset,0);
        }
    }
}


