using Unity.VisualScripting;
using UnityEngine;

public class PlayButton: MonoBehaviour
{
    [SerializeField] private Camera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptToPressPlay();
        }
    }

    void AttemptToPressPlay()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2), 0);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.transform.CompareTag("Play"))
            {
                GameOptionsMenu();
            }
        }
    }

    void GameOptionsMenu()
    {
        
    }

    void ConnectToServer()
    {
        
    }
}
