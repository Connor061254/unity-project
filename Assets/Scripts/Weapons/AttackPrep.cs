using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackPrep : MonoBehaviour
{
    private float nextAttackTime = 0f;

    // 1. CREATE THE EMPTY BOX HERE 
    // (Inside the class, but outside of any methods)
    private OfficialPickupScript pickupScript; 

    public Camera MainCamera;
    public bool isAiming = false;

    private float aimStartTime = 0f;

    public float powerMultiplier;

    public Vector3 throwPosition;

    void Start()
    {
        // 2. FILL THE BOX HERE 
        // (Unity runs this once as soon as the game starts)
        pickupScript = GetComponent<OfficialPickupScript>();
    }

    void Update()
    {
        
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
             float cooldown = pickupScript.heldObject.GetComponent<RockWeapon>().attackCooldown;
            IWeapon weapon = pickupScript.heldObject.GetComponent<IWeapon>();

            if (weapon != null)
            {
                weapon.Attack();
                nextAttackTime = Time.time + cooldown; 
            }
        }
    }
    
    public void Throw()
    {
        var rockScript = pickupScript.GetComponent<RockWeapon>();

        if (rockScript != null)
        {
            rockScript.SetThrowPosition(pickupScript.heldObject.transform.position);
        }
        powerMultiplier = Time.time - aimStartTime;

        powerMultiplier = Mathf.Clamp(powerMultiplier, 0.5f, 3f);
        if(pickupScript.heldObject != null && UnityEngine.Time.time >= nextAttackTime)
        {

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
            IWeaponThrow throwWeapon = pickupScript.heldObject.GetComponent<IWeaponThrow>();

            if (throwWeapon != null)
            {
                throwWeapon.ThrowAttack(this.gameObject, targetPoint);
            }

            pickupScript.heldObject = null;
        }
    }
}