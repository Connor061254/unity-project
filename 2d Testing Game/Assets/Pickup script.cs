using UnityEngine;
using System.Collections.Generic; // Needed for using lists
using System.Linq; // Needed for handy list functions like OrderBy

public class PickUp : MonoBehaviour
{
    public Transform attachmentpoint;
    public float dropForce = 5f; // The force to apply when dropping the item
    public Camera playerCamera; // Reference to the player's camera

    private GameObject heldobject = null;
    
    // A list to keep track of all items currently in our pickup range
    private List<GameObject> pickupableItems = new List<GameObject>();

    // A flag to prevent instant re-pickup
    private bool canPickup = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldobject == null && pickupableItems.Count > 0 && canPickup)
            {
                // Find the closest item from the list before picking up
                PerformPickUp();
            }
            else if (heldobject != null)
            {
                Drop();
            }
        }
    }

    void PerformPickUp()
    {
        // Get the closest item to the player
        heldobject = GetClosestItem();
        if (heldobject == null) return; // Should not happen, but good practice

        // Remove the item from the list since we're now holding it
        pickupableItems.Remove(heldobject);

        Rigidbody rb = heldobject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        heldobject.transform.SetParent(attachmentpoint);
        heldobject.transform.localPosition = Vector3.zero;
        heldobject.transform.localRotation = Quaternion.identity; // Also reset rotation
    }

    void Drop()
    {
        // Start the cooldown coroutine
        StartCoroutine(DropCooldown());

        heldobject.transform.SetParent(null);
        Rigidbody rb = heldobject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        // Add a forward force to "throw" the item slightly
        // We use the camera's forward direction for more intuitive aiming
        rb.AddForce(playerCamera.transform.forward * dropForce, ForceMode.Impulse);

        heldobject = null;
    }

    // This finds the closest object from our list of available items
    private GameObject GetClosestItem()
    {
        // Use Linq to order items by their distance to the player and take the first one
        return pickupableItems.OrderBy(item => Vector3.Distance(item.transform.position, transform.position)).FirstOrDefault();
    }

    // Coroutine to handle the pickup cooldown
    private System.Collections.IEnumerator DropCooldown()
    {
        canPickup = false;
        // Wait for a short duration
        yield return new WaitForSeconds(0.5f);
        canPickup = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // If an item enters the trigger and isn't already in our list, add it
        if (other.CompareTag("Item") && !pickupableItems.Contains(other.gameObject))
        {
            pickupableItems.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If an item leaves the trigger, remove it from the list
        if (other.CompareTag("Item") && pickupableItems.Contains(other.gameObject))
        {
            pickupableItems.Remove(other.gameObject);
        }
    }
}