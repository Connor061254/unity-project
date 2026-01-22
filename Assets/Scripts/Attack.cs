using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    private float nextAttackTime = 0f;
    private float range = 2f;
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

        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject.GetComponent<HealthController>())
            {
                hit.gameObject.GetComponent<HealthController>().TakeDamage(10f);
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
        
    }
}
