using UnityEngine;

public class CameraBob : MonoBehaviour
{

    [SerializeField] private float walkingCameraBobFrequancy;

    [SerializeField] private float walkingCameraBobStrength;

    [SerializeField] private float runningCameraBobFrequancy;

    [SerializeField] private float runningCameraBobStrength;

    [SerializeField] private float movementSpeedBuffApply = 1f;

    [SerializeField] private float movementSpeedBuff = 2f;

    private float timer = 0f;

    private float runTimer = 0f;

    private Vector3 defaultPos;

    private Vector3 newPos;

    private OfficialPickupScript ops;

    [SerializeField] [Range(1,10)] private float smoothReturnSpeed = 5f;


    private bool isMoving = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       defaultPos = transform.localPosition; 
       ops = transform.parent.GetComponent<OfficialPickupScript>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCameraBob();

        if (isMoving == true)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                RunCameraBob();
            }
            else
            {
                StartCameraBob();
            }
            
        }
        else
        {
            ReturnCameraPos();
        }
    }

    void CheckCameraBob()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            isMoving = true;
        }

        else
        {
            isMoving = false;
        }
    }

    void StartCameraBob()
    {
        if( ops.heldObject != null && ops.heldObject.GetComponent<SpecialAbility>() != null && transform.parent.GetComponent<Identification>().type == CharacterType.TubbsMcGee)
        {
            
            movementSpeedBuffApply = movementSpeedBuff;
        }
        else
        {
            movementSpeedBuffApply = 1f;
        }

        timer += Time.deltaTime * (walkingCameraBobFrequancy * movementSpeedBuffApply);
        float newY = defaultPos.y + Mathf.Sin(timer) * (walkingCameraBobStrength * movementSpeedBuffApply);
        float newX = defaultPos.x + Mathf.Cos(timer/2) * (walkingCameraBobStrength * movementSpeedBuffApply);

        newPos = new Vector3(newX, newY, defaultPos.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * smoothReturnSpeed);
        
    }

    void ReturnCameraPos()
    {
        if (transform.localPosition != defaultPos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPos, Time.deltaTime * smoothReturnSpeed);
            timer = 0;
        }
    }

    void RunCameraBob()
    {
         if(ops.heldObject != null && ops.heldObject.GetComponent<SpecialAbility>() != null && transform.parent.GetComponent<Identification>().type == CharacterType.TubbsMcGee)
        {
            movementSpeedBuffApply = movementSpeedBuff;
        }
        else
        {
            movementSpeedBuffApply = 1f;
        }
        runTimer += Time.deltaTime * (runningCameraBobFrequancy * movementSpeedBuffApply);
        float newY = defaultPos.y + Mathf.Sin(runTimer) * (runningCameraBobStrength * movementSpeedBuffApply);
        float newX = defaultPos.x + Mathf.Cos(runTimer/2) * (runningCameraBobStrength * movementSpeedBuffApply);

        transform.localPosition = new Vector3(newX, newY, defaultPos.z);
    }
}
