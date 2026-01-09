using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(transform.parent == null)
        {
            return;

        }
        
        if(collision.gameObject == transform.parent.gameObject)
        {
            return;
        }
        
        if(collision.gameObject.GetComponent<HealthController>())
        {
             collision.gameObject.GetComponent<HealthController>().TakeDamage(10f);
        }
    

           
    }            
}
