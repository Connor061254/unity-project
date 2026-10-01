using System.Collections;
using Mono.CSharp;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchGameManager : NetworkBehaviour
{
    private bool hasStarted = false;

    private int clientsLoaded = 0;

    public NetworkVariable<int> countDown= new NetworkVariable<int>(5);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnClientLoadedScene;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && NetworkManager.Singleton != null && NetworkManager.Singleton.SceneManager != null)
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnClientLoadedScene;
        }
    }

    void Start()
    {
        lockMovementRpc();
    }

    void OnClientLoadedScene(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        clientsLoaded++;

        if(clientsLoaded == NetworkManager.Singleton.ConnectedClients.Count)
        {
            StartCoroutine(PreMatchCountDown());
        }
    }

    IEnumerator PreMatchCountDown()
    {
        while (countDown.Value > 0)
        {
            yield return new WaitForSeconds(1f);
            countDown.Value--;
        }

        StartMatchRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    void StartMatchRpc()
    {
        PlayerController[] Players = FindObjectsByType<PlayerController>();

        foreach (PlayerController player in Players)
        {
            if (player.IsOwner)
            {
                player.matchStarted = true;
                break;
            }
        }
    }

[Rpc(SendTo.ClientsAndHost)]
    void lockMovementRpc()
    {
         PlayerController[] Players = FindObjectsByType<PlayerController>();

        foreach (PlayerController player in Players)
        {
            if (player.IsOwner)
            {
                player.matchStarted = false;
                break;
            }
        }
    }
}
