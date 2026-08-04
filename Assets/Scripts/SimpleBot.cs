using NUnit.Framework;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components; // Added this to access NetworkTransform
using UnityEngine;
using UnityEngine.AI;

public class SimpleBot : NetworkBehaviour
{
    public Transform targetPlayer;

    private Transform currentTarget;
    private NavMeshAgent agent;
    private Transform targetWeapon;

    private Transform targetBot;

    public Transform weaponHolder;

    private bool hasWeapon = false;
    
    // NEW: We will store the actual 3D model we want to hold here
    private Transform heldWeaponTransform;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if(!IsServer && agent != null)
        {
            agent.enabled = false;
        }
    }

    void Update()
    {
        if(!IsServer) return;

        DetermineTarget();

        if (currentTarget != null)
        {
            agent.destination = currentTarget.position;
        }
    }

       

    void DetermineTarget()
    {
        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;

        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in allPlayers)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if(distToPlayer < closestDistance)
            {
                closestDistance = distToPlayer;
                bestTarget = player.transform;
            }
        }

        if (!hasWeapon)
        {
            DamageDealer[] allWeapons = GameObject.FindObjectsByType<DamageDealer>();

            foreach (DamageDealer weapon in allWeapons)
            {
                float distToWeapon = Vector3.Distance(transform.position, weapon.transform.position);

                if(distToWeapon < closestDistance)
                {
                    closestDistance = distToWeapon;
                    bestTarget = weapon.transform;
                }
            }
        }

        currentTarget = bestTarget;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!IsServer) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Attack Player");
        }

        if(hasWeapon == false)
        {
            if(other.TryGetComponent<NetworkObject>(out NetworkObject rockNetObj) && other.TryGetComponent<RockWeapon>(out RockWeapon weapon))
            {
                Debug.Log("ran into rock");
                
                if(other.TryGetComponent(out Rigidbody rockRb)) rockRb.isKinematic = true;
                if(other.TryGetComponent(out Collider rockCollider)) rockCollider.enabled = false;

                // Server makes the rock a child of the BOT (Legal Network Move)
                if (rockNetObj.TrySetParent(this.transform, false))
                {
                    Debug.Log("parented object");
                    DisableWeaponsPhysicsRpc(rockNetObj);
                }
                else
                {
                    Debug.Log("no network object");
                }
                hasWeapon = true;  
            }
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    void DisableWeaponsPhysicsRpc(NetworkObjectReference networkObject)
    {
        if(networkObject.TryGet(out NetworkObject weapon))
        {
            if(weapon.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) rigidbody.isKinematic = true;
            if(weapon.TryGetComponent<Collider>(out Collider collider)) collider.enabled = false;

            // Turn off the NetworkTransform so the server stops fighting our visual position!
            if (weapon.TryGetComponent<NetworkTransform>(out NetworkTransform netTransform)) netTransform.enabled = false;

            // INSTEAD of parenting, we save the transform reference
            heldWeaponTransform = weapon.transform;
        }
    }

    // NEW: LateUpdate runs AFTER all animations and physics for the frame.
    // This perfectly snaps the weapon to the hand on everyone's screen.
    void LateUpdate()
    {
        // Notice there is NO "if(!IsServer) return;" here! 
        // We want this visual snapping to run on all clients.
        if (heldWeaponTransform != null && weaponHolder != null)
        {
            heldWeaponTransform.position = weaponHolder.position;
            heldWeaponTransform.rotation = weaponHolder.rotation;
        }
    }
}