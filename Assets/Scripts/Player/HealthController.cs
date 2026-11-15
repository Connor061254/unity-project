using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class HealthController : NetworkBehaviour
{

    [SerializeField] private NetworkVariable<float> maxPlayerHealth = new NetworkVariable<float>(100);

    [SerializeField] private NetworkVariable<float> currentPlayerHealth = new NetworkVariable<float>(100);


    private GameObject myHealthBar;

    private Rigidbody[] ragdollRigidbodies;

    public Collider rootCollider;

    private Collider[] ragdollColliders;
    private HealthbarUI healthScript;
    public GameObject hatPrefab;
    public bool isDowned;
    public float outwardForce = 3f;

    public float upwardForce = 5f;
    public override void OnNetworkSpawn()
    {
        healthScript = FindAnyObjectByType<HealthbarUI>(FindObjectsInactive.Include);

        if(healthScript != null)
        {
            myHealthBar = healthScript.gameObject; 
            healthScript.SetMaxHealth(maxPlayerHealth.Value);
            healthScript.SetHealth(currentPlayerHealth.Value);
        }

        // This is the magic alarm clock! 
        // Anytime the server changes health, run the UpdateHealthUI function automatically.
        currentPlayerHealth.OnValueChanged += UpdateHealthAndCheckDeath;

        // Only the server is allowed to set the starting values
        if (IsServer)
        {
            currentPlayerHealth.Value = maxPlayerHealth.Value;
        }
    }

    private void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        ragdollColliders = GetComponentsInChildren<Collider>();

        SetRagdollState(false);
    }

    public void Update()
    {
        // testing
        if (Input.GetKey(KeyCode.Comma))
        {
            TakeDamage(100f);
        }
        if (Input.GetKey(KeyCode.Period))
        {
            GetComponent<Points>().points.Value++;
        }
        if (Input.GetKey(KeyCode.Tab))
        {
            Revive();
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
            if(this.TryGetComponentInChildren<Animator>(out Animator animator))
            {
                animator.enabled = false;
            }
            
            SetRagdollState(true);
            if(TryGetComponent<PlayerController>(out PlayerController playerController) && TryGetComponent<Look>(out Look look))
            {
                playerController.enabled = false;
                look.enabled = false;
            }

            isDowned = true;

           // StartCoroutine(Respawn());
        }
    }

    public void Revive()
    {
        if(currentPlayerHealth.Value > 0) return;

        SetRagdollState(false);

        if(this.TryGetComponentInChildren<Animator>(out Animator animator))
        {
            animator.enabled = true;
        }
       
        if(TryGetComponent<PlayerController>(out PlayerController playerController) && TryGetComponent<Look>(out Look look))
        {
            playerController.enabled = true;
            look.enabled = true;
        }

        isDowned = false;
    }

    public void TakeDamage(float damageAmount) 
    {
        // All the client does is ask the server to handle the math
        RequestHealthTakeDamageRpc(damageAmount);
    }

    [Rpc(SendTo.Server)]
    private void RequestHealthTakeDamageRpc(float damageAmount)
    {
        currentPlayerHealth.Value -= damageAmount;

        if (currentPlayerHealth.Value <= 0)
        {
            Points getPoints = GetComponent<Points>();
            int hatsToDrop = getPoints.points.Value;

            if (hatsToDrop > 0)
            {
                DropHatsServerSide(transform.position, hatsToDrop);
            }
            else
            {
                Debug.Log("no points");
            }
            
            getPoints.RemovePoints();
        }
    }

    // Notice there's no RPC tag here, because we only call this from inside the damage RPC (which is already on the server)
    private void DropHatsServerSide(Vector3 dropPosition, int amountOfHats)
    {
        Debug.Log("hat drop called");
        for (int i = 0; i < amountOfHats; i++)
        {
            Debug.Log("enough points");
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                1f,
                Random.Range(-1f, 1f)
            );

            float randomX = Random.Range(-1f,1f);
            float randomZ = Random.Range(-1f,1f);
            Vector3 sproutDirection = new Vector3(randomX * outwardForce, upwardForce, randomZ * outwardForce);

            GameObject spawnedhat = Instantiate(hatPrefab, dropPosition + randomOffset, Quaternion.identity);
            spawnedhat.GetComponent<NetworkObject>().Spawn();

            spawnedhat.GetComponent<Rigidbody>().AddForce(sproutDirection, ForceMode.Impulse);
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

        if (IsServer)
        {
             currentPlayerHealth.Value = 100f;
        }
       
    }

    private void SetRagdollState(bool isRagdollActive)
    {
        foreach (Rigidbody r in ragdollRigidbodies)
        {
           r.isKinematic = !isRagdollActive;
           r.useGravity = isRagdollActive;
        }

        foreach (Collider c in ragdollColliders)
        {
            if(c == rootCollider) continue;

            c.enabled = isRagdollActive;
        }
    }
}
