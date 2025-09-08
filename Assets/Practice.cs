using UnityEngine;

public class Practice : MonoBehaviour
{
    private GameObject heldobject;
    private bool canPickup = true;

    public Camera playercamera;

    public float dropforce = 5;

    public float throwforce = 15;

    public Transform Attatchmentpoint;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldobject == null && canPickup)
            {
                trytopickup();
            }
            else if (heldobject != null)
            {
                drop();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (heldobject != null)
            {
                throwobject();
            }
        }
    }

    void trytopickup()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, transform.forward, out hitInfo, 5f);

        if (hitInfo.collider.CompareTag("Item"))
        {
            performpickup(hitInfo.collider.gameObject);
        }
    }

    void drop()
    {
        Rigidbody rb = heldobject.transform.GetComponent<Rigidbody>();
        heldobject.transform.SetParent(null);
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(transform.position * dropforce, ForceMode.Impulse);
        heldobject = null;
    }

    void performpickup(GameObject objectToPickup)
    {
        heldobject = objectToPickup;
        Rigidbody rb = heldobject.transform.GetComponent<Rigidbody>();
        heldobject.transform.SetParent(Attatchmentpoint);
        rb.useGravity = false;
        rb.isKinematic = true;
        heldobject.transform.localPosition = Vector3.zero;
        

    }

    void throwobject()
    {
        Rigidbody rb = heldobject.transform.GetComponent<Rigidbody>();
        heldobject.transform.SetParent(null);
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(playercamera.transform.forward * throwforce, ForceMode.Impulse);
        heldobject = null;

    }
    
}
