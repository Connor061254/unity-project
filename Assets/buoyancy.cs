using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MultiFloaterBuoyancy : MonoBehaviour
{
    // You can drag your floater objects onto this in the Inspector
    public Transform[] floaters;

    public float underwaterDrag = 3f;
    public float underwaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;
    public float waterHeight = 0f;

    private Rigidbody rb;
    private int floatersUnderwater;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        floatersUnderwater = 0;

        // Apply force at each floater's position
        foreach (Transform floater in floaters)
        {
            float difference = floater.position.y - waterHeight;

            if (difference < 0)
            {
                // Apply the buoyant force at the floater's position
                rb.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floater.position, ForceMode.Force);
                floatersUnderwater++;
            }
        }

        // Change the drag properties based on whether ANY floater is underwater
        if (floatersUnderwater > 0)
        {
            rb.linearDamping = underwaterDrag;
            rb.angularDamping = underwaterAngularDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
            rb.angularDamping = airAngularDrag;
        }
    }
}