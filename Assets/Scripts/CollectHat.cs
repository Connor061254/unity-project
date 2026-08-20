using Unity.InferenceEngine;
using Unity.Netcode;
using UnityEngine;

public class CollectHat : NetworkBehaviour
{ 
    [SerializeField] private Transform hatPosition;

    [SerializeField] private GameObject hatPrefab;

     private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

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
