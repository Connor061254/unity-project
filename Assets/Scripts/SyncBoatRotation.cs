using Unity.VisualScripting;
using UnityEngine;

public class SyncBoatRotation : MonoBehaviour
{

    [SerializeField] private Transform physicsBoat;

    void LateUpdate()
    {
        if (physicsBoat == null) return;

        Vector3 waterTilt = transform.eulerAngles;

        float physicsSteering = physicsBoat.eulerAngles.y;

        transform.eulerAngles = new Vector3(waterTilt.x, physicsSteering, waterTilt.z);
    }
}
