using Unity.Netcode;
using UnityEngine;

public class Points : NetworkBehaviour
{
   public NetworkVariable<int> points = new NetworkVariable<int>(0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            RequestAddPointRpc();
        }
        
    }

    [Rpc(SendTo.Server)]
    private void RequestAddPointRpc()
    {
        points.Value = 1;
    }

    [Rpc(SendTo.Server)]
    public void RequestRemovePointRpc()
    {
        points.Value = 0;
    }
}
