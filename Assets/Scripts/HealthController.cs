using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float maxPlayerHealth = 100f;
    [SerializeField] private float currentPlayerHealth = 100f;

    private GameObject myHealthBar;

    private HealthbarUI healthScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myHealthBar = GameObject.FindWithTag("HealthUI");
        healthScript = myHealthBar.GetComponent<HealthbarUI>();
        currentPlayerHealth = maxPlayerHealth;
        healthScript.SetMaxHealth(maxPlayerHealth);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log($"[HealthController] OUCH! I took {damageAmount} damage. Current Time: {Time.time}");
        currentPlayerHealth -= damageAmount;
        healthScript.SetHealth(currentPlayerHealth);

        if(currentPlayerHealth <= 0f)
        {
           Destroy(gameObject);
        }

        Debug.Log($"[HealthController] Health remaining: {currentPlayerHealth}");

     
    }

    
}
