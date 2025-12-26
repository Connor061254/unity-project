using UnityEngine;

public class CameraBob : MonoBehaviour
{

    [SerializeField] private float walkingCameraBobFrequancy;

    [SerializeField] private float walkingCameraBobStrength;

    private float timer = 0f;

    private Vector3 defualtPos;

    [SerializeField] [Range(1,10)] private float smoothReturnSpeed = 5f;


    private bool isMoving = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       defualtPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCameraBob();

        if (isMoving == true)
        {
            StartCameraBob();
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
        float newY = defualtPos.y + Mathf.Sin(timer) * walkingCameraBobStrength;
        float newX = defualtPos.x + Mathf.Cos(timer/2) * walkingCameraBobStrength;

        transform.localPosition = new Vector3(newX, newY, defualtPos.z);
    }

    void ReturnCameraPos()
    {
        if (transform.localPosition != defualtPos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defualtPos, Time.deltaTime * smoothReturnSpeed);
            timer = 0;
        }
    }
}
