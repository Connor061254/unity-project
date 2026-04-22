using UnityEngine;

public class PushPhysicsObjects : MonoBehaviour
{
    public float pushPower = 2f;
  
  private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var body = hit.collider.attachedRigidbody;

        if (body == null)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.AddForceAtPosition(pushDirection * pushPower, hit.point, ForceMode.Impulse);
    }
}
