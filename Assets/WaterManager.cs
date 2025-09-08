using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public static WaterManager instance;

    public float waveHeight = 0.5f;
    public float waveFrequency = 0.5f;
    public float waveSpeed = 1f;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    public float GetWaterHeightAtPosition(Vector3 position)
    {
        float time = Time.time * waveSpeed;
        float x = position.x * waveFrequency;
        float z = position.z * waveFrequency;
        float noise = Mathf.PerlinNoise(x + time, z + time);
        return (noise - 0.5f) * waveHeight * 2f;
    }
}