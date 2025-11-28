using UnityEditor.Rendering.Universal;
using UnityEngine;

public class crouch : MonoBehaviour


{
    private PlayerController controller;

    public float crouchSpeed = 2f;

    public float crouchHeight = 4f;

    public float walkSpeed;

    public float standingHeight;

    public Camera playerCamera;




        void Start()
    {
        controller = GetComponent<PlayerController>();
        standingHeight = controller.standingHeight;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouch();
        }

        else
        {
            StandUp();
        }
    }

    void Crouch()
    {
        standingHeight = crouchHeight;
        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, crouchHeight, playerCamera.transform.position.z);
        controller.walkSpeed = crouchSpeed;
    }

    void StandUp()
    {
        
    }
    

}
