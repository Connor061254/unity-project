using UnityEngine;

public class WaveManagerTest : MonoBehaviour
{
    // --- Singleton Pattern ---
    // The type has been updated to WaveManagerTest
    public static WaveManagerTest instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // --- Wave Parameters ---
    // These public variables will appear in the Unity Inspector,
    // allowing you to change them without editing the code.
    [Header("Wave 1 (e.g., Gradient Noise)")]
    public float scale1 = 0.2f;
    public float speed1 = 0.5f;
    public float amplitude1 = 0.1f;
    public Vector2 direction1 = new Vector2(1, 0.5f);

    [Header("Wave 2 (e.g., Voronoi Noise)")]
    public float scale2 = 0.8f;
    public float speed2 = 1.2f;
    public float amplitude2 = 0.05f;
    public Vector2 direction2 = new Vector2(-0.5f, 1);

    /// <summary>
    /// Calculates the combined wave height at a specific world position.
    /// </summary>
    public float GetWaveDisplacement(float x, float z)
    {
        // Calculate scrolling offset for each wave
        float offsetX1 = Time.time * speed1 * direction1.x;
        float offsetY1 = Time.time * speed1 * direction1.y;
        
        float offsetX2 = Time.time * speed2 * direction2.x;
        float offsetY2 = Time.time * speed2 * direction2.y;

        // Sample the Perlin noise for each wave
        float noiseValue1 = Mathf.PerlinNoise(
            (x + offsetX1) * scale1,
            (z + offsetY1) * scale1
        );
        
        float noiseValue2 = Mathf.PerlinNoise(
            (x + offsetX2) * scale2,
            (z + offsetY2) * scale2
        );

        // Map noise from [0, 1] to [-1, 1]
        noiseValue1 = (noiseValue1 * 2) - 1;
        noiseValue2 = (noiseValue2 * 2) - 1;
        
        // Calculate final height by adding the waves together
        float totalHeight = (noiseValue1 * amplitude1) + (noiseValue2 * amplitude2);

        return totalHeight;
    }

    // --- Editor Gizmo ---
    // Draws a helper visualization in the Scene view.
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0.5f, 0.75f);
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}