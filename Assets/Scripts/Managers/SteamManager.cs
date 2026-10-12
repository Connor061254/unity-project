using UnityEngine;
using Steamworks;

public class SteamManager : MonoBehaviour
{
    private void Awake()
    {
        try
        {
            SteamClient.Init(480);
            Debug.Log("Steam initialized succesfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error initializing Steam: " + e.Message);
        }

        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        SteamClient.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        SteamClient.Shutdown();
    }

}
