using UnityEngine;

public class WaterManager : MonoBehaviour
{
    // A static reference so any script can easily access this manager
    public static WaterManager instance;

    public float waveHeight = 0.5f;
    public float waveFrequency = 0.5f;
    public float waveSpeed = 1f;

    private void Awake()
    {
        // Set up the singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // This is the function other scripts will call
    public float GetWaterHeightAtPosition(Vector3 position)
    {
        // Use Perlin noise to create a natural-looking wave pattern
        // The time component makes the waves move
        float time = Time.time * waveSpeed;
        float x = position.x * waveFrequency;
        float z = position.z * waveFrequency;

        // Get the Perlin noise value, which is between 0 and 1
        float noise = Mathf.PerlinNoise(x + time, z + time);

        // Return the noise value scaled by our waveHeight
        // We subtract 0.5 to have the waves go both up and down
        return (noise - 0.5f) * waveHeight * 2f;
    }
}