using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class HealthController : NetworkBehaviour
{

    [SerializeField] private NetworkVariable<float> maxPlayerHealth = new NetworkVariable<float>(100);

    [SerializeField] private NetworkVariable<float> currentPlayerHealth = new NetworkVariable<float>(100);


    private GameObject myHealthBar;
    private HealthbarUI healthScript;
    public GameObject hatPrefab;

    // Use OnNetworkSpawn instead of Start
    public override void OnNetworkSpawn()
    {
        myHealthBar = GameObject.FindWithTag("HealthUI");
        healthScript = myHealthBar.GetComponent<HealthbarUI>();
        
        healthScript.SetMaxHealth(maxPlayerHealth.Value);
        healthScript.SetHealth(currentPlayerHealth.Value);

        // This is the magic alarm clock! 
        // Anytime the server changes health, run the UpdateHealthUI function automatically.
        currentPlayerHealth.OnValueChanged += UpdateHealthAndCheckDeath;

        // Only the server is allowed to set the starting values
        if (IsServer)
        {
            currentPlayerHealth.Value = maxPlayerHealth.Value;
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Comma))
        {
            currentPlayerHealth.Value = 0;
        }
    }

    public override void OnNetworkDespawn()
    {
        // Always unsubscribe when destroyed to prevent memory leaks
        currentPlayerHealth.OnValueChanged -= UpdateHealthAndCheckDeath; 
    }

    // This runs automatically whenever currentPlayerHealth.Value changes!
    private void UpdateHealthAndCheckDeath(float previousValue, float newValue)
    {
        healthScript.SetHealth(newValue);
        Debug.Log($"[HealthController] Health changed from {previousValue} to {newValue}");

        // If health hits 0, trigger the local death visuals
        if (newValue <= 0 && previousValue > 0)
        {
            var getPoints = GetComponent<Points>();
            getPoints.RemovePoints(); 

            if(GetComponent<MeshRenderer>() && GetComponent<Collider>().enabled)
            {
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
            }
            

            if(TryGetComponent<PlayerController>(out PlayerController playerController) && TryGetComponent<Look>(out Look look))
            {
                playerController.enabled = false;
                look.enabled = false;
            }

            StartCoroutine(Respawn());
        }
    }

    // 2. Changed float to int to match your NetworkVariable
    public void TakeDamage(float damageAmount) 
    {
        // All the client does is ask the server to handle the math
        RequestHealthTakeDamageRpc(damageAmount);
    }

    // 3. Added the missing RPC tag!
    [Rpc(SendTo.Server)]
    private void RequestHealthTakeDamageRpc(float damageAmount)
    {
        currentPlayerHealth.Value -= damageAmount;

        // Since we are already on the Server inside this RPC, we can just 
        // handle the hat dropping and point removal directly! No extra RPCs needed.
        if (currentPlayerHealth.Value <= 0)
        {
            Points getPoints = GetComponent<Points>();
            int hatsToDrop = getPoints.points.Value;

            if (hatsToDrop > 0)
            {
                DropHatsServerSide(transform.position, hatsToDrop);
            }
            
            getPoints.points.Value = 0;
        }
    }

    // Notice there's no RPC tag here, because we only call this from inside the damage RPC (which is already on the server)
    private void DropHatsServerSide(Vector3 dropPosition, int amountOfHats)
    {
        for (int i = 0; i < amountOfHats; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                1f,
                Random.Range(-1f, 1f)
            );

            GameObject spawnedhat = Instantiate(hatPrefab, dropPosition + randomOffset, Quaternion.identity);
            spawnedhat.GetComponent<NetworkObject>().Spawn();
        }
    }
    
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;

        if(TryGetComponent<PlayerController>(out PlayerController component) && TryGetComponent<Look>(out Look lComponent))
        {
            component.enabled = true;
            lComponent.enabled = true;
        }

        currentPlayerHealth.Value = 100f;
    }
}
