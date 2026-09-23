using UnityEngine;
using Unity.Netcode;
using System.Xml.Serialization;
using System.Collections;

public class ChampionSelectionUI : MonoBehaviour
{

    public GameObject healthUI;

    public GameObject crossHairUI;
    public void SelectTallPirate()
    {
       StartCoroutine(StartIslandAndSelect(1));
    }

    public void SelectFatPirate()
    {
        StartCoroutine(StartIslandAndSelect(0));
    }

    public void SelectWomenPirate()
    {
        StartCoroutine(StartIslandAndSelect(2));
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

    private IEnumerator StartIslandAndSelect(int championIndex)
    {
        StartIsland();

        while (NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject() == null)
        {
            yield return null;
        }

        SelectChampion(championIndex);
        
        gameObject.SetActive(false);
    }

    private void StartIsland()
    {
        NetworkManager.Singleton.StartHost();

        if(healthUI != null) healthUI.SetActive(true);
        if(crossHairUI != null) crossHairUI.SetActive(true);
    }
}
