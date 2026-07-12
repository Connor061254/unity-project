using Unity.Netcode;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if(velocity.magnitude > 0.1f)
        {
           CollisionFlags flags = controller.Move(velocity * Time.deltaTime);

        // The proper way to check if a CharacterController is alive and active
            bool isControllerActive = controller.enabled && controller.gameObject.activeInHierarchy;

        // This will print to your console the exact millisecond you shoot
            Debug.Log($"[Knockback Engine] Moving! Vector: {velocity * Time.deltaTime} | " +
                  $"Controller Active: {isControllerActive} | " +
                  $"Collision Flags: {flags}");
        }

        
       
        velocity = Vector3.Lerp(velocity, Vector3.zero, 4f * Time.deltaTime);
    }

    public void AddKnockBack(float force, Vector3 direction)
    {
        velocity += direction * force;
    }
}
