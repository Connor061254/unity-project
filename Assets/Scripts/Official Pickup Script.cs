using UnityEditor;
using UnityEngine;

public class OfficialPickupScript : MonoBehaviour
{
    public Transform holdPosition;
    public float pickupRange = 2f;
    public float throwForce = 8f;
    public GameObject heldObject;
    private bool canPickup = true;

    public ItemData itemInfo;

    public float dropForce = 2f;
    Rigidbody rb;
    public Camera mainCamera;

    private InteractableItem currentTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (heldObject == null && canPickup)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                AttemptToPickup();
            }
        }
        else if (heldObject != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Drop();
            }
        }
    }
    void FixedUpdate()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickupRange))
        {
                var hitItem = hit.collider.GetComponent<InteractableItem>();
                
                if(hitItem != null)
                {
                    if(currentTarget != hitItem)
                    {
                        if(currentTarget != null) currentTarget.HidePrompt();

                        currentTarget = hitItem;
                        currentTarget.ShowPrompt();

                    }
                }
                else
                {
                    if(currentTarget != null)
                    {
                        currentTarget.HidePrompt();
                        currentTarget = null;
                    }
                }
        
        }
    }
    void AttemptToPickup()
    {
        int layerMask = 1 << 6;
        layerMask = ~layerMask;
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickupRange, layerMask))
        {
            Debug.Log("My Raycast hit: " + hit.collider.name);

            if (hit.transform != null && hit.transform.CompareTag("PickUp"))
            {
                heldObject = hit.transform.gameObject;

                var itemProp = heldObject.GetComponent<ItemProperties>();
                var inventory = this.gameObject.GetComponent<InventoryManager>();

                if(inventory != null && itemProp != null)
                {
                    itemInfo = itemProp.referenceData;
                    
                    bool wasPickedUp = inventory.AddItem(itemInfo);

                    if (wasPickedUp)
                    {
                        PerformPickup();
                    }
                    else
                    {
                        heldObject = null;
                    }

                }
                
            }
        }
    }

    void PerformPickup()
    {
        heldObject.transform.SetParent(holdPosition);
        var itemSettings = heldObject.GetComponent<ItemPickupPosition>();
        if(itemSettings != null)
        {
            heldObject.transform.localPosition = itemSettings.positionOffset;
            heldObject.transform.localRotation = Quaternion.Euler(itemSettings.rotationOffset);
        }

        else
        {
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;
        }
       
        
        Rigidbody objectRb = heldObject.GetComponent<Rigidbody>();
        objectRb.isKinematic = true;
        objectRb.useGravity = false;
    }

    void Drop()
    {
        StartCoroutine(DropCooldown());
        var inventory = this.gameObject.GetComponent<InventoryManager>();
        itemInfo = heldObject.GetComponent<ItemProperties>().referenceData;
        inventory.RemoveItem(itemInfo);
        heldObject.transform.SetParent(null);
        Rigidbody objectRb = heldObject.GetComponent<Rigidbody>();
        objectRb.isKinematic = false;
        objectRb.useGravity = true;
        objectRb.AddForce(mainCamera.transform.forward * dropForce, ForceMode.VelocityChange);
        heldObject = null;
    }

    System.Collections.IEnumerator DropCooldown()
    {
        canPickup = false;
        yield return new WaitForSeconds(0.5f); 
        canPickup = true;
    }

    
    
}

