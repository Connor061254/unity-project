using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float maxPlayerHealth = 100f;
    [SerializeField] private float currentPlayerHealth = 100f;

    private float damage = 10f;

    public bool takingDamage = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void takeDamage()
    {
        if (takingDamage == true)
        {
            currentPlayerHealth -= damage;

            if(currentPlayerHealth <= 0f)
            {
                Destroy(this);
            }

        }
    }
}
