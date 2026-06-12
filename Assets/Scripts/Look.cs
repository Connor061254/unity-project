using UnityEngine;

public class Look : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 500f;

    float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if(Cursor.lockState == CursorLockMode.Locked)
        {
             float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= y;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        

            playerBody.Rotate(Vector3.up * x);
        }
       
    }
}
