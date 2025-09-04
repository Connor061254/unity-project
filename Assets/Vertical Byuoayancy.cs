using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleVerticalFloat : MonoBehaviour
{
    [Header("Float Settings")]
    [Tooltip("An offset to adjust how high the object sits in the water. A positive value lifts it higher.")]
    [SerializeField] private float floatHeightOffset = 0.0f;

    [Tooltip("How smoothly the object follows the water's movement. Higher values are more responsive.")]
    [SerializeField] private float movementSmothness = 5f;
    
    [Tooltip("How smoothly the object levels itself with the water. Higher values are more responsive.")]
    [SerializeField] private float rotationSmothness = 5f;


    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Using gravity can interfere with direct position control, so we disable it.
        rb.useGravity = false;
        // We'll be controlling rotation, so it's best to freeze it here to prevent physics interference.
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (VerticalWaveManager.instance == null)
        {
            return;
        }

        // --- POSITION CONTROL ---
        
        // Get the current, uniform height of the water plane.
        float waveHeight = VerticalWaveManager.instance.GetWaveHeight(transform.position) + 
                           VerticalWaveManager.instance.transform.position.y;

        // Calculate the target position for this object.
        Vector3 targetPosition = new Vector3(
            rb.position.x,                               // Keep the current X position.
            waveHeight + floatHeightOffset,              // Set the Y position to the water level plus an offset.
            rb.position.z                                // Keep the current Z position.
        );

        // Smoothly interpolate to the target position.
        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, movementSmothness * Time.fixedDeltaTime));

        // --- ROTATION CONTROL ---

        // Calculate the target rotation to be perfectly level (no pitch or roll).
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        // Smoothly interpolate to the target rotation.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmothness * Time.fixedDeltaTime);
    }
}

