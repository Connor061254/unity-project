using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MultiFloaterBuoyancy : MonoBehaviour
{
    public Transform[] floaters;

    public float underwaterDrag = 3f;
    public float underwaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;
    // We no longer need the public waterHeight variable
    // public float waterHeight = 0f;

    private Rigidbody rb;
    private int floatersUnderwater;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        floatersUnderwater = 0;
        
        foreach (Transform floater in floaters)
        {
            // --- THIS IS THE KEY CHANGE ---
            // Ask the WaterManager for the current water height at the floater's position
            float waterHeight = WaterManager.instance.GetWaterHeightAtPosition(floater.position);

            float difference = floater.position.y - waterHeight;

            if (difference < 0)
            {
                rb.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floater.position, ForceMode.Force);
                floatersUnderwater++;
            }
        }

        if (floatersUnderwater > 0)
        {
            rb.drag = underwaterDrag;
            rb.angularDrag = underwaterAngularDrag;
        }
        else
        {
            rb.drag = airDrag;
            rb.angularDrag = airAngularDrag;
        }
    }
}