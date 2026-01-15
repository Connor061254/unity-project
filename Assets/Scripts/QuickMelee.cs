using UnityEngine;
using UnityEngine.InputSystem;

public class QuickMelee : MonoBehaviour
{
    float range = 2f;
  
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Melee()
    {
        Vector3 attackPoint = transform.position + (transform.forward * range);
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint, 1f);

        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject.GetComponent<HealthController>())
            {
                hit.gameObject.GetComponent<HealthController>().TakeDamage(100f);
            }
        }
        
    }

    void OnMelee(InputValue value)
    {
        if (value.isPressed && GetComponent<OfficialPickupScript>().heldObject != null)
        {
            Melee();
        }
    }
}


