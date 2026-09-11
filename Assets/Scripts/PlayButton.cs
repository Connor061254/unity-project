using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private string matchScene = "matchScene";

    private GameObject gameManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptToPressPlay();
        }
    }

    public override void OnNetworkSpawn()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager");
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
                CreateTeams();
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

    void CreateTeams()
    {
        List<ulong> allPlayers = new List<ulong>();

        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            allPlayers.Add(clientId);
        }
            
        gameManager.GetComponent<GameManager>().AssignSoloPlayer(allPlayers);

        StartDemoMatch();
    }

    public void StartDemoMatch()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.SceneManager.LoadScene("matchScene", LoadSceneMode.Single);
    }

}