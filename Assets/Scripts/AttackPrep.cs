using UnityEngine;
using UnityEngine.InputSystem;

public class AttackPrep : MonoBehaviour
{
    private float nextAttackTime = 0f;
    public float throwForce = 20f;
    public Camera MainCamera;

    // 1. CREATE THE EMPTY BOX HERE 
    // (Inside the class, but outside of any methods)
    private OfficialPickupScript pickupScript; 

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
    
    // ... your OnThrow and Throw methods go below here! ...
}