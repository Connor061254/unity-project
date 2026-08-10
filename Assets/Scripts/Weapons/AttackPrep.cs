using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackPrep : MonoBehaviour
{
    private float nextAttackTime = 0f;
    private OfficialPickupScript pickupScript; 

    public Camera MainCamera;
    public bool isAiming = false;
    private float aimStartTime = 0f;
    public float powerMultiplier;
    private float cooldown;
    private TrajectoryPredictor trajectoryPredictor;
    private float baseThrowSpeed = 10f;

    void Start()
    {
        pickupScript = GetComponent<OfficialPickupScript>();
        trajectoryPredictor = GetComponent<TrajectoryPredictor>();
    }

    void Update()
    {
        if (isAiming && pickupScript != null && pickupScript.heldObject != null)
        {
            // 1. Calculate the real-time power multiplier
            float currentPower = Mathf.Clamp(Time.time - aimStartTime, 0.5f, 2f);

            // 2. Find the target point 
            Ray ray = MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(100f);
            }

            // 3. Calculate the velocity dynamically right NOW
            Vector3 startPos = pickupScript.heldObject.transform.position;
            Vector3 currentVelocity = CalculateThrowVelocity(currentPower);

            // 4. Draw the line!
            if (trajectoryPredictor != null)
            {
                trajectoryPredictor.UpdateTrajectory(startPos, currentVelocity);
            }
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAiming = true;
            aimStartTime = Time.time;
        }

        if (context.canceled)
        {
            isAiming = false;
            powerMultiplier = 0;

            if(trajectoryPredictor != null)
            {
                trajectoryPredictor.ClearTrajectory();
            }
        }
    }

    public void OnPrimaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isAiming)
            {
                Throw();
            }
            else
            {
                Melee();
            }
        }
    }

    public void Melee()
    {
        if (pickupScript != null && pickupScript.heldObject != null && Time.time >= nextAttackTime)
        {
            if (pickupScript.heldObject.GetComponent<RockWeapon>())
            {
                cooldown = pickupScript.heldObject.GetComponent<RockWeapon>().attackCooldown;
            }
            else
            {
                cooldown = 2f;
            }
             
            IWeapon weapon = pickupScript.heldObject.GetComponent<IWeapon>();

            if (weapon != null)
            {
                weapon.Attack(gameObject);
                nextAttackTime = Time.time + cooldown; 
            }
        }
    }
    
    public void Throw()
    {
        var rockScript = pickupScript.heldObject.GetComponent<RockWeapon>();

        if (rockScript != null)
        {
            rockScript.SetThrowPosition(pickupScript.heldObject.transform.position);
        }
        
        powerMultiplier = Mathf.Clamp(Time.time - aimStartTime, 0.5f, 2f);
        
        if(pickupScript.heldObject != null && Time.time >= nextAttackTime)
        {
            Ray ray = MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Vector3 targetPosition;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPosition = hit.point;
            }
            else
            {
                targetPosition = ray.GetPoint(100f);
            }
            
            IWeaponThrow throwWeapon = pickupScript.heldObject.GetComponent<IWeaponThrow>();

            if (throwWeapon != null)
            {
                NetworkObject netObj = GetComponent<NetworkObject>();
                Vector3 startPosition = pickupScript.heldObject.transform.position;
                
                // Calculate the final velocity at the exact moment of the throw
               Vector3 finalVelocity = CalculateThrowVelocity(powerMultiplier);
                throwWeapon.ThrowAttack(netObj, finalVelocity);
            }

            pickupScript.heldObject = null;

            if(trajectoryPredictor != null)
            {
                trajectoryPredictor.ClearTrajectory();
            }

            isAiming = false;
        }
    }

    private Vector3 CalculateThrowVelocity(float power)
    {
        // Prevent division by zero if power gets weird
       float currentSpeed = baseThrowSpeed * power;

        // 2. Base the throw entirely on where the player's eyes are looking
        Vector3 throwDirection = MainCamera.transform.forward;

        // 3. Add a generous, consistent upward arc. 
        // This ensures a low-power throw arcs nicely over the ground instead of slamming into your toes.
        throwDirection += Vector3.up * 0.35f; 
        
        throwDirection.Normalize();

        return throwDirection * currentSpeed;
    }
}