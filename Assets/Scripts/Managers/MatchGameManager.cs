using System.Collections;
using Mono.CSharp;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchGameManager : NetworkBehaviour
{
    private bool hasStarted = false;

    private int clientsLoaded = 0;

    public float delayBeforeEvent = 600f;

    public NetworkVariable<double> eventTime = new NetworkVariable<double>();

    public NetworkVariable<int> countDown= new NetworkVariable<int>(5);

    private bool hasEventTriggered = false;

    public Transform specialHatTransform;

    public GameObject specialHat;
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

    void Update()
    {
        if(eventTime.Value == 0 || hasEventTriggered == false) return;

        if(NetworkManager.Singleton.ServerTime.Time >= eventTime.Value)
        {
            hasEventTriggered = true;

            if (IsServer)
            {
                TriggerSpecialHatEvent();
            }
        }
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

                if (IsServer)
                {
                     eventTime.Value = NetworkManager.Singleton.ServerTime.Time + delayBeforeEvent;
                }
               
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

    void TriggerSpecialHatEvent()
    {
        GameObject eventHat = Instantiate(specialHat, specialHatTransform.position, Quaternion.identity);

        NetworkObject networkObject = eventHat.GetComponent<NetworkObject>();
        networkObject.Spawn();
    }
}
