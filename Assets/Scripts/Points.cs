using Unity.AppUI.UI;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Points : NetworkBehaviour
{
    [SerializeField] private TeamName teamName;
    public NetworkVariable<int> points = new NetworkVariable<int>(0);
    public NetworkVariable<int> team1Points = new NetworkVariable<int>();
    public NetworkVariable<int> team2Points = new NetworkVariable<int>();
    public NetworkVariable<int> team3Points = new NetworkVariable<int>();
    public NetworkVariable<int> team4Points = new NetworkVariable<int>();
    public NetworkVariable<int> team5Points = new NetworkVariable<int>();


    private void Update()
    {
        CheckForWin();
    } 
    public override void OnNetworkSpawn()
    {
        if (IsOwner && IsLocalPlayer)
        {
            RequestAddPointRpc();
        }
        
    }

    [Rpc(SendTo.Server)]
    private void RequestAddPointRpc()
    {
        points.Value = 1;
        AddPoints();
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

        var hat = GetComponent<StackHats>();

        if(hat != null)
        {
             hat.RequestStackHatsRpc();
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
            team1Points.Value -= points.Value;
            break;

            case TeamName.team2:
            team2Points.Value -= points.Value;
            break;

            case TeamName.team3:
            team3Points.Value -= points.Value;
            break;

            case TeamName.team4:
            team4Points.Value -= points.Value;
            break;

            case TeamName.team5:
            team5Points.Value -= points.Value;
            break;
        }

        points.Value = 0;
    }

    private void CheckForWin()
    {
        switch (team1Points.Value)
        {
            case 15:
            //Temporary (need to add an endscreen showing who won)
            NetworkManager.Singleton.Shutdown();
            break;
        }
        switch (team2Points.Value)
        {
            case 15:
            //Temporary (need to add an endscreen showing who won)
            NetworkManager.Singleton.Shutdown();
            break;
        }
         switch (team3Points.Value)
        {
            case 15:
            //Temporary (need to add an endscreen showing who won)
            NetworkManager.Singleton.Shutdown();
            break;
        }
         switch (team4Points.Value)
        {
            case 15:
            //Temporary (need to add an endscreen showing who won)
            NetworkManager.Singleton.Shutdown();
            break;
        }
         switch (team5Points.Value)
        {
            case 15:
            //Temporary (need to add an endscreen showing who won)
            NetworkManager.Singleton.Shutdown();
            break;
        }
    }
}
