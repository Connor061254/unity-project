using Unity.InferenceEngine;
using UnityEngine;

public class CollectHat : MonoBehaviour
{
     private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player found the hat");
            var playerPoints = other.gameObject.GetComponent<Points>();

            ++playerPoints.points.Value;
            playerPoints.AddPoints();

            Debug.Log("points added");

            Destroy(gameObject);
        }
    }
}
