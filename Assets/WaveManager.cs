using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // Singleton instance to ensure only one WaveManager exists and is easily accessible.
    public static WaveManager instance;

    [Header("Wave Settings")]
    [Tooltip("The primary amplitude (height) of the waves.")]
    [SerializeField] private float amplitude = 1f;

    [Tooltip("The primary wavelength (distance between crests).")]
    [SerializeField] private float wavelength = 2f;

    [Tooltip("How fast the waves travel.")]
    [SerializeField] private float speed = 1f;

    [Tooltip("The direction the waves are traveling in.")]
    [SerializeField] private Vector2 direction = new Vector2(1, 0);

    [Header("Gerstner Wave Settings (for sharper crests)")]
    [Tooltip("The steepness of the wave crests. 0 is a sine wave, 1 is a sharp crest.")]
    [Range(0, 1)]
    [SerializeField] private float steepness = 0.5f;

    // Ensure there is only one instance of the WaveManager.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Another instance of WaveManager exists, destroying this one.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Calculates the wave displacement at a given world position.
    /// </summary>
    /// <param name="position">The world position to sample.</param>
    /// <returns>A Vector3 containing the wave displacement (x, y, z).</returns>
    public Vector3 GetWaveDisplacement(Vector3 position)
    {
        // Normalize the direction vector.
        Vector2 normalizedDirection = direction.normalized;
        
        // Gerstner wave calculation
        float k = 2 * Mathf.PI / wavelength;
        float c = Mathf.Sqrt(9.8f / k); // Wave speed based on gravity and wave number
        
        // Directional vector for wave calculation
        Vector2 d = normalizedDirection;

        float f = k * (Vector2.Dot(d, new Vector2(position.x, position.z)) - c * Time.time * speed);
        
        float a = steepness / k;

        // Calculate horizontal (x, z) and vertical (y) displacement
        float horizontalDisplacementX = a * Mathf.Cos(f) * d.x;
        float verticalDisplacement = amplitude * Mathf.Sin(f);
        float horizontalDisplacementZ = a * Mathf.Cos(f) * d.y;
        
        return new Vector3(horizontalDisplacementX, verticalDisplacement, horizontalDisplacementZ);
    }
    
    /// <summary>
    /// Gets only the wave height at a specific world position.
    /// </summary>
    /// <param name="position">The world position to sample.</param>
    /// <returns>The height of the wave at that position.</returns>
    public float GetWaveHeight(Vector3 position)
    {
        return GetWaveDisplacement(position).y;
    }
}
