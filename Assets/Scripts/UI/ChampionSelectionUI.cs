using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using Netcode.Transports.Facepunch;
using Steamworks;

public class ChampionSelectionUI : MonoBehaviour
{
    private FacepunchTransport transport;
    public GameObject healthUI;
    public GameObject crossHairUI;  
    
    private void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<FacepunchTransport>();
    }

    public void SelectTallPirate() => _ = HandleCharacterSelectionAsync(1);
    public void SelectFatPirate() => _ = HandleCharacterSelectionAsync(0);
    public void SelectWomenPirate() => _ = HandleCharacterSelectionAsync(2);

    private async Task HandleCharacterSelectionAsync(int championIndex)
    {
        // Only try to start a new host lobby if we aren't already connected as a client
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            bool success = await StartIslandAsync();
            if (!success) return; 
        }

        // Wait for Netcode to spawn the local player object
        while (NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject() == null)
        {
            await Task.Yield(); 
        }

        SelectChampion(championIndex);
        
        // Turn on gameplay UI
        if(healthUI != null) healthUI.SetActive(true);
        if(crossHairUI != null) crossHairUI.SetActive(true);
        
        // Hide this selection screen
        gameObject.SetActive(false);
    }

    private void SelectChampion(int championIndex)
    {
        NetworkObject playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        if (playerObject == null) return;

        PlayerDataBackpack playerData = playerObject.GetComponent<PlayerDataBackpack>();
        if (playerData != null)
        {
            playerData.SelectChampion(championIndex);
        }
    }

    private async Task<bool> StartIslandAsync() 
    {
        Debug.Log("Requesting Steam to create a lobby...");
        var lobby = await SteamMatchmaking.CreateLobbyAsync(4);

        if (!lobby.HasValue)
        {
            Debug.LogError("Failed to create Steam Lobby");
            return false;
        }

        Debug.Log("Steam Lobby created successfully.");
        lobby.Value.SetPublic();
        lobby.Value.SetJoinable(true);

        transport.targetSteamId = SteamClient.SteamId;
        NetworkManager.Singleton.StartHost();

        return true;
    }
}