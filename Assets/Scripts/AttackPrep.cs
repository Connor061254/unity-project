using JetBrains.Annotations;
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
     void OnThrow(InputValue value)
    {
        if(value.isPressed && GetComponent<OfficialPickupScript>().heldObject != null && UnityEngine.Time.time >= nextAttackTime)
        {
            Throw();
        }
    }

    void Throw()
    {
        
        Debug.Log("Camera is pointing towards: " + MainCamera.transform.forward);

        var pickupScript = GetComponent<OfficialPickupScript>();
        var obj = pickupScript.heldObject;
        var rb = obj.GetComponent<Rigidbody>();

        var weaponScript = obj.GetComponent<RockWeapon>();

        if(weaponScript != null)
        {
            weaponScript.lastOwner = this.gameObject;
        }

        obj.transform.SetParent(null);
        rb.useGravity = true;
        rb.isKinematic = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

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

        Vector3 throwDirection = (targetPoint - obj.transform.position).normalized;

        rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);
    }
}