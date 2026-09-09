using UnityEngine;
using Unity.Netcode;

public class PlayerDataBackpack : NetworkBehaviour
{
    public GameObject fatPiratePrefab;

    public GameObject womanPiratePrefab;

    public GameObject tallPiratePrefab;

    private NetworkObject currentLobbyChamp;

    public NetworkVariable<int> SelectedChampIndex = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    );

    public NetworkVariable<int> TeamIndex = new NetworkVariable<int>(-1,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        SelectedChampIndex.OnValueChanged += OnChampSelectionChanged;
    }

    public override void OnNetworkDespawn()
    {
        SelectedChampIndex.OnValueChanged -= OnChampSelectionChanged;
    }

    private void OnChampSelectionChanged(int previousValue, int newValue)
    {
        RequestChangeChampRpc(newValue);
    }

    [Rpc(SendTo.Server)]
    private void RequestChangeChampRpc(int requestedChamp)
    {
        SelectedChampIndex.Value = requestedChamp;

        if (currentLobbyChamp != null)
        {
            currentLobbyChamp.Despawn();
        }

        GameObject prefabToSpawn = null;

        switch (requestedChamp)
        {
            case 0: prefabToSpawn = fatPiratePrefab; break;
            case 1: prefabToSpawn = tallPiratePrefab; break;
            case 2: prefabToSpawn = womanPiratePrefab; break;
        }

        if (prefabToSpawn != null)
        {
            GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");
            Vector3 startPosition = Vector3.zero;
            if(spawnPoint != null)
            {
                startPosition = spawnPoint.transform.position;
            }
            GameObject fatPirate = Instantiate(prefabToSpawn, startPosition, Quaternion.identity);
            currentLobbyChamp = fatPirate.GetComponent<NetworkObject>();
            currentLobbyChamp.SpawnWithOwnership(OwnerClientId);
        }
    }
}
