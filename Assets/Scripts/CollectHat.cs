using Unity.InferenceEngine;
using UnityEngine;

public class CollectHat : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"colission triggered with {collision.gameObject.name}");
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            Debug.Log($"{collision.gameObject.name} has a PlayerController");
            var playerPoints = collision.gameObject.GetComponent<Points>();

            ++playerPoints.points.Value;
            
            Destroy(gameObject);
        }
    }
}
