using UnityEngine;
using Unity.Netcode;

public class GameMapSpawner : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;

        SpawnAllPlayers();
    }

    private void SpawnAllPlayers()
    {
        foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            NetworkManager.Singleton.ConnectedClients.TryGetValue(clientID, out NetworkClient client);

            PlayerDataBackpack backpack = client.PlayerObject.GetComponent<PlayerDataBackpack>();

            if(backpack != null)
            {
                float TeamId = backpack.TeamIndex.Value;

                GameObject spawnLocation = GameObject.FindWithTag("Team" + TeamId + "Spawn");

                Vector3 spawnPos = Vector3.zero;
                if(spawnLocation != null)
                {
                    Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                    spawnPos = spawnLocation.transform.position + offset;
                }

                backpack.SpawnAvatarInGame(spawnPos);
            }
        }
    }
}
