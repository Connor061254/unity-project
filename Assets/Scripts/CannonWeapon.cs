using UnityEngine;

// The cannon signs the IWeapon contract
public class CannonWeapon : MonoBehaviour, IWeapon 
{
    [Header("Cannon Setup")]
    public GameObject cannonballPrefab; 
    public Transform firePoint;

         
    
    [Header("Firing Settings")]
    public float fireForce = 2000f;     
    public float recoilForce = 40f; 

    // This is the method the player script will trigger when you press the "Melee" button
    public void Attack()
    {
        // 1. Spawn the cannonball
        GameObject newCannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
        var ballRb = newCannonball.GetComponent<Rigidbody>();
        
        if (ballRb != null)
        {
            // Shoot it forward!
            ballRb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);
        }

        // 2. Apply Recoil to the Player
        // Since the cannon is currently a child of the player's HoldPoint, 
        // GetComponentInParent will find the player's Rigidbody.
        var playerKnockback = GetComponentInParent<KnockBack>();

        if (playerKnockback != null)
        {
            Vector3 recoilDirection = -firePoint.forward;
            playerKnockback.AddKnockBack(recoilForce, recoilDirection);
        }
    }
}