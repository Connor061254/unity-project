using Unity.Netcode;
using UnityEngine;

public class BillBoardUI : NetworkBehaviour
{
    private Transform mainCameraTransform;

    private Transform position;

    public Vector3 offset = new Vector3(0,0.3f,0);
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

        if (position != null)
        {
            transform.position = position.position + offset;

            transform.LookAt(transform.position + mainCameraTransform.forward);
        }
        else
        {
            Debug.Log("object position is null");
        }
    }
        

    public void GetObjectPosition(Transform objecttransform)
    {
        position = objecttransform;
    }
}
