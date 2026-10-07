using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    [Header("Egg Settings")]
    public string eggID = "Blue_Egg"; 
    
    public TelemetrySender telemetrySystem; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player found the " + eggID + "!");

            telemetrySystem.SendEggData("Gamer123", eggID);

            Destroy(gameObject);
        }
    }
}