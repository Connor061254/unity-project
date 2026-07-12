using Unity.Netcode;
using UnityEngine;

// The cannon signs the IWeapon contract
public class CannonWeapon : NetworkBehaviour, IWeapon 
{
    [Header("Cannon Setup")]
    public GameObject cannonballPrefab; 
    public Transform firePoint;

         
    
    [Header("Firing Settings")]
    public float fireForce = 2000f;     
    public float recoilForce = 40f; 

    private KnockBack assingedKnockback;

    // This is the method the player script will trigger when you press the "Melee" button
    public void Attack()
    {
        NetworkObject playerNetObj = GetComponentInParent<NetworkObject>();
        if (IsOwner && playerNetObj != null)
        {
            ApplyRecoil();
        }
       PerformShootCannonRpc();
    }

    public void SetupWeaponOwner(KnockBack playerKockback)
    {
        assingedKnockback = playerKockback;
    }

    private void ApplyRecoil()
    {
        Debug.Log("Recoil Applied!");
        var playerKnockback = assingedKnockback;
            if(playerKnockback != null)
            {
                Vector3 recoilDirection = -firePoint.forward;
                playerKnockback.AddKnockBack(recoilForce, recoilDirection);
                Debug.Log("Successfully called AddKnockBack!");
            }
        else
        {
            Debug.LogError("CRITICAL: Weapon could not find the KnockBack script in its parents!");
        }
    }

    [Rpc(SendTo.Server)]
    private void PerformShootCannonRpc()
    {
        GameObject newCannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
        NetworkObject netObj = newCannonball.GetComponent<NetworkObject>();

        if(netObj != null)
        {
            netObj.Spawn();
        }
        
        var ballRb = newCannonball.GetComponent<Rigidbody>();
        
        if (ballRb != null)
        {
            // Shoot it forward!
            ballRb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);
        }
    }
}