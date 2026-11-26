using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Summoner : NetworkBehaviour
{
    public GameObject capybaraPrefab;
    private Vector3 spawnPosition;

    private NetworkList<ulong> usedClientIds;

    public bool playerHasUsed = false;

    void Awake()
    {
        usedClientIds = new NetworkList<ulong>();
    }

    [Rpc(SendTo.Server)]
    public void TrySpawnCapybaraRpc(ulong clientId, RpcParams rpcParams = default)
    {
        if(rpcParams.Receive.SenderClientId != clientId) return;

        if (usedClientIds.Contains(clientId))
        {
            return;
        }

        usedClientIds.Add(clientId);

        SpawnCapybara();
    }
    public void SpawnCapybara()
    {
        if(playerHasUsed) return;

        var cameraTransform = Camera.main.transform;
        spawnPosition = cameraTransform.position + cameraTransform.forward * 2f;

        GameObject capybara = Instantiate(capybaraPrefab, spawnPosition, Quaternion.identity);

        capybara.TryGetComponent<NetworkObject>(out NetworkObject netCapybara);
        netCapybara.Spawn();

        playerHasUsed = true;
    }
}
