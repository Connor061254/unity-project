using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    private float nextAttackTime = 0f;
    private float range = 2f;

    public float throwForce = 20f;

    public Camera MainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMelee(InputValue value)
    {

        if (value.isPressed && GetComponent<OfficialPickupScript>().heldObject != null && Time.time >= nextAttackTime )
        {
            Melee();
        }
    }

    void Melee()
    {
        float delay = 1f;
        Vector3 attackPoint = transform.position + (transform.forward * range);
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint, 1f);
        nextAttackTime = UnityEngine.Time.time + delay;

        var enemiesHitThisSwing = new System.Collections.Generic.List<HealthController>();

        

        foreach (Collider hit in hitColliders)
        {
            Debug.Log("I hit: " + hit.name + " on object: " + hit.transform.root.name);
            var enemy = hit.GetComponentInParent<HealthController>();

            if (enemy != null)
            {
                if (!enemiesHitThisSwing.Contains(enemy))
                {
                    enemy.TakeDamage(10f);

                    enemiesHitThisSwing.Add(enemy);
                }
            
            }
        }
    }

    void OnThrow(InputValue value)
    {
        if(value.isPressed && GetComponent<OfficialPickupScript>().heldObject != null && UnityEngine.Time.time >= nextAttackTime)
        {
            Throw();
        }
    }

    void Throw()
    {
        var pickupScript = GetComponent<OfficialPickupScript>();
        pickupScript.heldObject.transform.SetParent(null);
        pickupScript.heldObject.GetComponent<Rigidbody>().useGravity = true;
        pickupScript.heldObject.GetComponent<Rigidbody>().isKinematic = false;
        pickupScript.heldObject.GetComponent<Rigidbody>().AddForce(MainCamera.transform.forward * throwForce, ForceMode.VelocityChange);
        
        

    }
}
