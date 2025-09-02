using UnityEngine;

public class WaterBuoyancyf : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;

    // Update is called once per frame
    private void FixedUpdate()
    {
    
        if (transform.position.y < 1)
        {
            float displacementMultiplier = Mathf.Clamp01(- transform.position.y / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }
}
