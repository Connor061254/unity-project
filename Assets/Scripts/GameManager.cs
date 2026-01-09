using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    public Transform spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawn()
    {
        Instantiate(player, spawnPoint.position, Quaternion.identity);
    }
}
