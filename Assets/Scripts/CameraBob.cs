using UnityEngine;

public class CameraBob : MonoBehaviour
{

    [SerializeField] private float walkingCameraBobFrequancy;

    [SerializeField] private float walkingCameraBobStrength;

    [SerializeField] private float runningCameraBobFrequancy;

    [SerializeField] private float runningCameraBobStrength;

    private float timer = 0f;

    private float runTimer = 0f;

    private Vector3 defaultPos;

    [SerializeField] [Range(1,10)] private float smoothReturnSpeed = 5f;


    private bool isMoving = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       defaultPos = transform.localPosition;
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
        timer += Time.deltaTime * walkingCameraBobFrequancy;
        float newY = defaultPos.y + Mathf.Sin(timer) * walkingCameraBobStrength;
        float newX = defaultPos.x + Mathf.Cos(timer/2) * walkingCameraBobStrength;

        transform.localPosition = new Vector3(newX, newY, defaultPos.z);
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
        runTimer += Time.deltaTime * runningCameraBobFrequancy;
        float newY = defaultPos.y + Mathf.Sin(runTimer) * runningCameraBobStrength;
        float newX = defaultPos.x + Mathf.Cos(runTimer/2) * runningCameraBobStrength;

        transform.localPosition = new Vector3(newX, newY, defaultPos.z);
    }
}
