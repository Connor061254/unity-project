using UnityEngine;

public class OfficialPickupScript : MonoBehaviour
{
    public Transform holdPosition;
    public float pickupRange = 3f;
    public float throwForce = 8f;
    private GameObject heldObject;
    private bool canPickup = true;

    public float dropForce = 2f;
    Rigidbody rb;
    public Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
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

            if (Input.GetMouseButtonDown(1))
            {
                Throw();
            }
        }
    }

    void AttemptToPickup()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.transform != null && hit.transform.CompareTag("PickUp"))
            {
                heldObject = hit.transform.gameObject;
                PerformPickup();
            }
        }
    }

    void PerformPickup()
    {
        heldObject.transform.SetParent(holdPosition);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
        Rigidbody objectRb = heldObject.GetComponent<Rigidbody>();
        objectRb.isKinematic = true;
        objectRb.useGravity = false;
    }

    void Drop()
    {
        StartCoroutine(DropCooldown());
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

    void Throw()
    {
        if (heldObject != null)
        {
            Rigidbody objectRb = heldObject.GetComponent<Rigidbody>();
            heldObject.transform.SetParent(null);
            objectRb.isKinematic = false;
            objectRb.useGravity = true;
            objectRb.AddForce(mainCamera.transform.forward * throwForce, ForceMode.VelocityChange);
            heldObject = null;
        }
    }
}

