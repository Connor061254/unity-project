using Unity.Netcode;
using UnityEngine;

public class BillBoardUI : NetworkBehaviour
{
    private Transform mainCameraTransform;

    private Vector3 offset = new Vector3(0,1.5f,0);
    // Start is called once before the first execution of Update after the MonoBehaviour is create

    // Update is called once per frame
    void LateUpdate()
    {
        if(mainCameraTransform == null)
        {
            if(Camera.main != null)
            {
                mainCameraTransform = Camera.main.transform;
            }
            else
            {
                return;
            }

             
        }

        transform.position = transform.parent.position + offset;

        transform.LookAt(transform.position + mainCameraTransform.forward);
    }
}
