using UnityEngine;

public class VerticalWaveManager : MonoBehaviour
{
    // Singleton instance to ensure only one WaveManager exists and is easily accessible.
    public static VerticalWaveManager instance;

    [Header("Wave Settings")]
    [Tooltip("The amplitude (height) of the waves.")]
    [SerializeField] private float amplitude = 1f;

    [Tooltip("The wavelength (distance between crests). NOTE: This is not used in the purely vertical mode.")]
    [SerializeField] private float wavelength = 5f;

    [Tooltip("How fast the waves travel.")]
    [SerializeField] private float speed = 1.5f;

    // Ensure there is only one instance of the WaveManager.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Another instance of VerticalWaveManager exists, destroying this one.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Calculates the wave displacement. In this version, it's uniform across the entire plane.
    /// This version only produces vertical displacement, ignoring the position parameter.
    /// </summary>
    /// <param name="position">The world position to sample (ignored in this implementation).</param>
    /// <returns>A Vector3 containing only the vertical (y) wave displacement.</returns>
    public Vector3 GetWaveDisplacement(Vector3 position)
    {
        // The core sine wave calculation, driven only by time.
        // This makes the entire water plane move up and down uniformly, preventing any rocking.
        float waveOffset = Mathf.Sin(Time.time * speed);

        float verticalDisplacement = amplitude * waveOffset;

        // Return a Vector3 with only the y-component changed for pure vertical motion.
        return new Vector3(0, verticalDisplacement, 0);
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