using UnityEngine;

public class CameraBob : MonoBehaviour
{

    private float walkingCameraBobFrequancy;

    private float walkingCameraBobStrength;

    private float timer = 0f;

    public float defualtPosY = 0f;

    public float defualtPosX = 0f;

    private bool isMoving = false;

    private float horizontal;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       defualtPosY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCameraBob();

        if (isMoving == true)
        {
            StartCameraBob();
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
        float newY = defualtPosY + Mathf.Sin(timer) * walkingCameraBobStrength;
    }
}
