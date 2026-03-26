using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    [Header("Egg Settings")]
    // You can change this in the Unity Inspector for each egg! 
    // (e.g., "Egg_1", "Egg_2")
    public string eggID = "Blue_Egg"; 
    
    // Drag your GameObject that has the TelemetrySender script here
    public TelemetrySender telemetrySystem; 

    // This built-in Unity function runs the exact moment something touches the egg
    private void OnTriggerEnter(Collider other)
    {
        // Check if the thing that touched the egg is the Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player found the " + eggID + "!");

            // 1. Tell our Telemetry script to drop the file for Python!
            // (I'm hardcoding "Gamer123" here, but in a real game, you'd pull their actual save-file name)
            telemetrySystem.SendEggData("Gamer123", eggID);

            // 2. Destroy the egg so it disappears from the game world
            Destroy(gameObject);
        }
    }
}