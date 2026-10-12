using System;
using System.Threading.Tasks;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using Unity.Netcode;
using UnityEngine;

public class SteamInviteHandler : MonoBehaviour
{

    public GameObject championSelectionUI;
    private FacepunchTransport transport;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<FacepunchTransport>();

        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
    }

    private void OnDestroy()
    {
        SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;
    }

    private async void OnGameLobbyJoinRequested(Lobby lobby, SteamId friendId)
    {
        Debug.Log("Accepted invite! Shutting down local island...");

        NetworkManager.Singleton.Shutdown();

        await Task.Delay(500);

        RoomEnter joinedLobby = await lobby.Join();

        if(joinedLobby != RoomEnter.Success)
        {
            Debug.LogError("Failed to join Steam lobby");
            return;
        }

       Debug.Log("Joined lobby! Connecting to friend's island...");

       transport.targetSteamId = lobby.Owner.Id;

       NetworkManager.Singleton.StartClient();

       if (championSelectionUI != null)
        {
            championSelectionUI.SetActive(true);
        }
    }
}
