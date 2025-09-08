using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{
    [Header("Buoyancy Settings")]
    [Tooltip("The strength of the buoyant force pushing the object up.")]
    [SerializeField] private float buoyancyStrength = 15f;
    
    [Tooltip("The strength of the damping force to slow down vertical movement.")]
    [SerializeField] private float waterDrag = 3f;

    [Tooltip("The strength of the damping force to slow down rotational movement.")]
    [SerializeField] private float waterAngularDrag = 1f;
    
    [Tooltip("An array of points on the object where buoyancy forces will be applied.")]
    [SerializeField] private Transform[] floaterPoints;

    [Tooltip("An offset to adjust how high the object sits in the water.")]
    [SerializeField] private float waveHeightOffset = 0.0f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (WaveManager.instance == null)
        {
            return;
        }

        int pointsSubmerged = 0;

        // Apply forces for each floater point
        foreach (Transform floater in floaterPoints)
        {
            Vector3 floaterPosition = floater.position;
            Vector3 waveDisplacement = WaveManager.instance.GetWaveDisplacement(floaterPosition);
            float waveHeight = waveDisplacement.y + WaveManager.instance.transform.position.y;
            
            // Check if the floater is below the wave
            if (floaterPosition.y < waveHeight + waveHeightOffset)
            {
                pointsSubmerged++;
                float submersion = (waveHeight + waveHeightOffset) - floaterPosition.y;
                
                // Calculate and apply buoyant force
                Vector3 buoyantForce = Vector3.up * buoyancyStrength * submersion;
                rb.AddForceAtPosition(buoyantForce, floaterPosition, ForceMode.Force);

                // Apply drag force to dampen movement
                Vector3 dragForce = -rb.GetPointVelocity(floaterPosition) * waterDrag * submersion;
                rb.AddForceAtPosition(dragForce, floaterPosition, ForceMode.Force);
                
                // Apply angular drag to dampen rotation
                Vector3 angularDragForce = -rb.angularVelocity * waterAngularDrag * submersion;
                rb.AddTorque(angularDragForce, ForceMode.Force);
            }
        }
    }

    // Visualize the floater points in the editor for easier setup
    private void OnDrawGizmosSelected()
    {
        if (floaterPoints == null) return;

        Gizmos.color = Color.cyan;
        foreach (Transform floater in floaterPoints)
        {
            if (floater != null)
            {
                Gizmos.DrawSphere(floater.position, 0.1f);
            }
        }
    }
}
