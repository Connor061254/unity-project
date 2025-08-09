using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform attachmentpoint;
    private GameObject pickupable = null;

    private GameObject heldobject = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldobject == null && pickupable != null)
            {
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
        heldobject = pickupable;
        Rigidbody rb = heldobject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        heldobject.transform.SetParent(attachmentpoint);
        heldobject.transform.localPosition = Vector3.zero;
    }

    void Drop()
    {
        heldobject.transform.SetParent(null);
        heldobject.GetComponent<Rigidbody>().useGravity = true;
        heldobject.GetComponent<Rigidbody>().isKinematic = false;
        heldobject = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            pickupable = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == pickupable)
        {
            pickupable = null;
        }
    }
}


