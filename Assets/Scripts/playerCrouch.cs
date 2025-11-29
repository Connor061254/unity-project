using UnityEngine;

public class playerCrouch : MonoBehaviour
{
    private PlayerController controller;

    public float crouchSpeed = 2f;

    public float crouchHeight = 4f;

    public float walkSpeed;

    public float standingHeight;

    public Camera playerCamera;

    private CharacterController characterController;




        void Start()
    {
        controller = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        standingHeight = characterController.height;
        
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
        playerCamera.transform.position = new UnityEngine.Vector3(playerCamera.transform.position.x, crouchHeight, playerCamera.transform.position.z);
        controller.walkSpeed = crouchSpeed;
    }

    void StandUp()
    {
        crouchHeight = standingHeight;
        playerCamera.transform.position = new UnityEngine.Vector3 (playerCamera.transform.position.x, standingHeight, playerCamera.transform.position.z);
        crouchSpeed = controller.walkSpeed;
        
    }
    
}
