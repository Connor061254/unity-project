using UnityEngine;

public class Practice : MonoBehaviour
{

    public float airdrag = 0f;
    public float waterdrag = 5f;
    public float airangulardrag = 0.05f;
    public float waterangulardrag = 0.5f;
    public float waterlevel = 0f;
    public float buoyancy = 1f;

    public transform[] floaters;

    public int floatersunderwater;

    rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (transform floater in floaters)
        {
            float difference = floater.position.y - waterlevel;
            if (difference > 0)
            {
                rb.AddForceAtPosition(Vector3.up * buoyancy * Mathf.Abs(difference), floater.position, ForceMode.force);
                floatersunderwater++;

                if (floatersunderwater > 0)
                {
                    rb.drag = waterdrag;
                    rb.angularDrag = waterangulardrag;
                }
                else
                {
                    rb.drag = airdrag;
                    rb.angularDrag = airangulardrag;
                }
            }
        }
        
    }
}
