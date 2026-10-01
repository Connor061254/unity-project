using Unity.Netcode;
using UnityEngine;

public class BillBoardUI : NetworkBehaviour
{
    private Transform mainCameraTransform;
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

       transform.LookAt(transform.position + mainCameraTransform.forward);
    }
}
