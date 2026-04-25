using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackPrep : MonoBehaviour
{
    private float nextAttackTime = 0f;

    // 1. CREATE THE EMPTY BOX HERE 
    // (Inside the class, but outside of any methods)
    private OfficialPickupScript pickupScript; 

    public Camera MainCamera;

    void Start()
    {
        // 2. FILL THE BOX HERE 
        // (Unity runs this once as soon as the game starts)
        pickupScript = GetComponent<OfficialPickupScript>();
    }

    void OnMelee(InputValue value)
    {
        // 3. USE THE BOX HERE
        // Notice we just say 'pickupScript.heldObject' instead of using GetComponent
        if (value.isPressed && pickupScript != null && pickupScript.heldObject != null && Time.time >= nextAttackTime)
        {
            IWeapon weapon = pickupScript.heldObject.GetComponent<IWeapon>();

            if (weapon != null)
            {
                weapon.Attack();
                nextAttackTime = Time.time + 2f; 
            }
        }
    }
    
     void OnThrow(InputValue value)
    {
        if(value.isPressed && GetComponent<OfficialPickupScript>().heldObject != null && UnityEngine.Time.time >= nextAttackTime)
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