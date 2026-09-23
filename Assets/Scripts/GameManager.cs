using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private int[] teamsPlayerCount = new int[5];

    public void AssignOnePlayer(ulong clientId, int teamIndex)
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
        {
            PlayerDataBackpack playerData = client.PlayerObject.GetComponent<PlayerDataBackpack>();

            if(playerData != null)
            {
                playerData.TeamIndex.Value = teamIndex;
            }
        }
    }

    public void AssignSoloPlayer(List<ulong> soloPlayers)
    {
        foreach (ulong clientId in soloPlayers)
        {
            int availableTeam = FindTeamWithSpace();

            if(availableTeam != -1)
            {
                AssignOnePlayer(clientId, availableTeam);
            }
            else
            {
                Debug.LogWarning($"Uh oh! All teams are full. Couldn't place Client {clientId}.");
            }
        }
    }

    private int FindTeamWithSpace()
    {
        for (int i = 0; i < 5; i++)
        {
            if (teamsPlayerCount[i] < 3)
            {
                teamsPlayerCount[i]++;
                return i;
            }
        }
        return -1;
    }
}
