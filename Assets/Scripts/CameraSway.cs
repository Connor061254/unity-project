using Mono.CSharp;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    [SerializeField] private float swayAmaount = 2f;

    [SerializeField] private float maxSway = 5f;

    [SerializeField] private float returnSpeed = 10f;

    private float currentTilt = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");

        float targetTilt = -mouseX * swayAmaount;

        targetTilt = Mathf.Clamp(targetTilt, -maxSway, maxSway);

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * returnSpeed);

        Vector3 currentRot = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentRot.x, currentRot.y, currentTilt);
    }
}
