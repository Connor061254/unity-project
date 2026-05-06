using UnityEngine;

public class BillBoardUI : MonoBehaviour
{
    private Transform mainCameraTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCameraTransform.forward);
    }
}
