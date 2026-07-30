using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class SimpleBot : NetworkBehaviour
{
    public Transform targetPlayer;
    private NavMeshAgent agent;
    private Transform targetWeapon;

    public Transform weaponHolder;

    private bool hasWeapon = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if(!IsServer && agent != null)
        {
            agent.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsServer) return;

        if(targetWeapon == null)
        {
            FindWeapon();
            return;
        }

        if (targetPlayer == null)
        {
            FindPlayer();
            return;
        }

        agent.destination = targetWeapon.position;

        if (hasWeapon)
        {
            agent.destination = targetPlayer.position;
        }
    }

    void FindPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if(player != null)
        {
            targetPlayer = player.transform;
        }
    }

    void FindWeapon()
    {
        var weapon = FindAnyObjectByType<RockWeapon>();

        if(weapon != null)
        {
            targetWeapon = weapon.transform;
        }
    }

    void OnTggerEnter(Collider other)
    {
        if(!IsServer) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Attack Player");
        }

        if(!hasWeapon)
        {
            if(other.TryGetComponent<NetworkObject>(out NetworkObject rockNetObj) && other.TryGetComponent<RockWeapon>(out RockWeapon weapon))
            {
                Debug.Log("ran into rock");
                
                if(other.TryGetComponent(out Rigidbody rockRb))
                {
                    rockRb.isKinematic = true;
                }

                if(other.TryGetComponent(out Collider rockCollider))
                {
                    rockCollider.isTrigger = true;
                }
                rockNetObj.TrySetParent(weaponHolder);

                other.transform.localPosition = Vector3.zero;
                other.transform.localRotation = Quaternion.identity;

                hasWeapon = true;
                
            }

           
        }
    }
}
