using UnityEngine;
using Unity.Netcode;
using System.Xml.Serialization;

public class ChampionSelectionUI : MonoBehaviour
{
    public void SelectTallPirate()
    {
        SelectChampion(1);
        StartIsland();
    }

    public void SelectFatPirate()
    {
        SelectChampion(0);
        StartIsland();
    }

    public void SelectWomenPirate()
    {
        SelectChampion(2);
        StartIsland();
    }

    private void SelectChampion(int championIndex)
    {
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsClient)
        {
            return;
        }

        NetworkObject playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        if (playerObject == null)
        {
            return;
        }

        PlayerDataBackpack playerData = playerObject.GetComponent<PlayerDataBackpack>();
        if (playerData != null)
        {
            playerData.SelectChampion(championIndex);
        }
    }

    private void StartIsland()
    {
        gameObject.SetActive(false);

        NetworkManager.Singleton.StartHost();

        GameObject.FindGameObjectWithTag("HealthUI").SetActive(true);
        GameObject.FindGameObjectWithTag("CrossHair").SetActive(true);
    }
}
