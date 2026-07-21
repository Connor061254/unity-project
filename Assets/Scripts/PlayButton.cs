using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private string matchScene = "matchScene";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptToPressPlay();
        }
    }

    void AttemptToPressPlay()
    {
        Debug.Log("E key pressed! Firing raycast...");

        if (playerCamera == null)
        {
            Debug.LogError("Camera is missing! Assign it in the Inspector.");
            return; 
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f, layerMask))
        {
            Debug.Log($"ray hit {hit.transform.gameObject}");
            if (hit.transform.CompareTag("Play"))
            {
                RequestMatchTransfer();
            }
            else
            {
                Debug.Log($"Hit {hit.transform.name}, but it lacks the 'Play' tag!");
            }
        }
        else
        {
            Debug.Log("Raycast fired but missed everything on the layer mask");
        }
    }

    void RequestMatchTransfer()
    {
        // Spawns a temporary persistent object to run the coroutine safely
        GameObject runner = new GameObject("HostResetRunner");
        DontDestroyOnLoad(runner);
        runner.AddComponent<HostResetRunner>().StartReset(matchScene);
    }
}

// Separate helper class that won't get destroyed during NetworkManager.Shutdown()
public class HostResetRunner : MonoBehaviour
{
    public void StartReset(string sceneName)
    {
        StartCoroutine(ConnectToServerRoutine(sceneName));
    }

    private IEnumerator ConnectToServerRoutine(string sceneName)
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            Debug.Log("Shutting down current host...");
            NetworkManager.Singleton.Shutdown();
        }

        // Wait until NetworkManager has fully finished shutting down
        while (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            yield return null;
        }

        Debug.Log($"Loading local scene: {sceneName}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (NetworkManager.Singleton != null)
        {
            Debug.Log("Starting host in new scene!");
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            Debug.LogError("NetworkManager missing in new scene! Ensure it has DontDestroyOnLoad attached.");
        }

        // Destroy this helper runner object now that the process is finished
        Destroy(gameObject);
    }
}