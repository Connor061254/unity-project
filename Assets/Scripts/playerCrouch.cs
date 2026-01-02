using UnityEngine;

public class playerCrouch : MonoBehaviour
{
    private PlayerController controller;

    public float crouchSpeed = 2f;

    public float crouchHeight = 4f;

    private float crouchCamera;

    public float crouchCameraHeight = 0.4f;

    private float standingCamera;

    public float crouchTransitionSpeed = 10f;

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

        Debug.Log(standingCamera);
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
        characterController.height = Mathf.Lerp(characterController.height, crouchHeight, Time.deltaTime * crouchTransitionSpeed);
        characterController.center = Vector3.Lerp(characterController.center, crouchCenter, Time.deltaTime * crouchTransitionSpeed);
        controller.walkSpeed = crouchSpeed;

        float currentCamY = playerCamera.transform.localPosition.y;
        float newCamY = Mathf.Lerp(currentCamY, crouchCameraHeight, Time.deltaTime * crouchTransitionSpeed);
        playerCamera.transform.localPosition = new UnityEngine.Vector3(playerCamera.transform.localPosition.x, newCamY, playerCamera.transform.localPosition.z);
        Debug.Log("stand Y is" + standingCamera);
        Debug.Log("Crouch Targer is :" + crouchCameraHeight);
        
    }

    void StandUp()
    {
        characterController.height = Mathf.Lerp(characterController.height, standingHeight, Time.deltaTime * crouchTransitionSpeed);
        characterController.center = Vector3.Lerp(characterController.center, standingCenter, Time.deltaTime * crouchTransitionSpeed);

        float currentCamY = playerCamera.transform.localPosition.y;
        float newCamY = Mathf.Lerp(currentCamY, standingCamera, Time.deltaTime * crouchTransitionSpeed);
        playerCamera.transform.localPosition = new UnityEngine.Vector3 (playerCamera.transform.localPosition.x, newCamY, playerCamera.transform.localPosition.z);
        controller.walkSpeed = WalkSpeed;
        
    }
    
}
