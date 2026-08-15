using Unity.Netcode;
using UnityEngine;

public class Points : NetworkBehaviour
{

    private TeamName teamName;
   public NetworkVariable<int> points = new NetworkVariable<int>(0);
   public NetworkVariable<int> team1Points = new NetworkVariable<int>();
   public NetworkVariable<int> team2Points = new NetworkVariable<int>();
   public NetworkVariable<int> team3Points = new NetworkVariable<int>();
   public NetworkVariable<int> team4Points = new NetworkVariable<int>();
   public NetworkVariable<int> team5Points = new NetworkVariable<int>();
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

    public void AddPoints()
    {
        if (!IsServer)
        {
            return;
        }

        switch (teamName)
        {
            case TeamName.team1:
            team1Points.Value++;
            break;

            case TeamName.team2:
            team2Points.Value++;
            break;

            case TeamName.team3:
            team3Points.Value++;
            break;

            case TeamName.team4:
            team4Points.Value++;
            break;

            case TeamName.team5:
            team5Points.Value++;
            break;
        }
    }

    public void RemovePoints()
    {
        if (!IsServer)
        {
            return;
        }

        switch (teamName)
        {
            case TeamName.team1:
            team1Points.Value--;
            break;

            case TeamName.team2:
            team2Points.Value--;
            break;

            case TeamName.team3:
            team3Points.Value--;
            break;

            case TeamName.team4:
            team4Points.Value--;
            break;

            case TeamName.team5:
            team5Points.Value--;
            break;
    }
    }
}
