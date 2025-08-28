using UnityEngine;

public class Practice : MonoBehaviour
{
    private GameObject heldobject;
    private bool canPickup = true;

    public Camera playercamera;

    public float dropforce = 5;

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
    }

    void trytopickup()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, transform.forward, out hitInfo, 5f);
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

    void performpickup()
    {

    }

    void throwobject()
    {
        
    }
    
}
