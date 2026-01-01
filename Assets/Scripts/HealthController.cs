using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float maxPlayerHealth = 100f;
    [SerializeField] private float currentPlayerHealth = 100f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void TakeDamage(float damageAmount)
    {
        currentPlayerHealth -= damageAmount;

        if(currentPlayerHealth <= 0f)
        {
           Destroy(gameObject);
        }

    
    }

    
}
