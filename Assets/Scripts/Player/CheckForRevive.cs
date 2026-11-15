using UnityEngine;
using UnityEngine.InputSystem;
public class CheckForRevive : MonoBehaviour
{
    public bool isReviving;
    private float reviveStartTime;
    private float timeHeld;

    private float reviveDuration = 4f;
    public Camera camera;

    public LayerMask targetLayerMask;
    public void OnRevive(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isReviving = true;
            reviveStartTime = Time.time;
        }

        if (context.canceled)
        {
            isReviving = false;
        }
    }

    public void Update()
    {
        if(!isReviving) return;

        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if(Physics.Raycast(ray, out RaycastHit hit, 2f, targetLayerMask))
        {
            var healthController = hit.transform.GetComponentInParent<HealthController>();

            if(!healthController) return;

            if(healthController.isDowned == true)
            {
                timeHeld = Time.time - reviveStartTime;
            }

            if(timeHeld >= reviveDuration)
            {
                healthController.Revive();
            }
        }
    }
}
