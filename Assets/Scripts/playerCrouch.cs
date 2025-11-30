using UnityEngine;

public class playerCrouch : MonoBehaviour
{
    private PlayerController controller;

    public float crouchSpeed = 2f;

    public float crouchHeight = 4f;

    private float crouchCamera;

    private float standingCamera;

    public float WalkSpeed;

    public float standingHeight;

    private Vector3 standingCenter;

    private Vector3 crouchCenter;

    public Camera playerCamera;

    private CharacterController characterController;




        void Start()
    {
        controller = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        standingHeight = characterController.height;
        standingCenter = characterController.center;
        WalkSpeed = controller.walkSpeed;
        standingCamera = playerCamera.transform.localPosition.y;

        crouchCenter = new Vector3(standingCenter.x, standingCenter.y - (standingHeight - crouchHeight) / 2f, standingCenter.z);
        
        crouchCamera = standingCamera - (standingHeight - crouchHeight);
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
        characterController.height = crouchHeight;
        characterController.center = crouchCenter;
        controller.walkSpeed = crouchSpeed;
        playerCamera.transform.localPosition = new UnityEngine.Vector3(playerCamera.transform.localPosition.x, crouchCamera, playerCamera.transform.localPosition.z);
        
    }

    void StandUp()
    {
        characterController.height = standingHeight;
        characterController.center = standingCenter;
        playerCamera.transform.localPosition = new UnityEngine.Vector3 (playerCamera.transform.localPosition.x, standingCamera, playerCamera.transform.localPosition.z);
        controller.walkSpeed = WalkSpeed;
        
    }
    
}
