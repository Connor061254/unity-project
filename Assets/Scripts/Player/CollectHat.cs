using System.Collections;
using System.Threading;
using Mono.CSharp;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CollectHat : NetworkBehaviour
{ 
    [SerializeField] private Transform hatPosition;

    [SerializeField] private GameObject hatPrefab;

    private bool isCollected = false;

    [SerializeField] private float moveSpeed = 10f;

     private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || isCollected) return;

        Debug.Log("triggered");
        if (other.CompareTag("Player"))
        {
            isCollected = true;
            Debug.Log("Player found the hat");

            GetComponent<Collider>().enabled = false;
            var rb = GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.isKinematic = true;
            }
            var playerPoints = other.gameObject.GetComponent<Points>();

            ++playerPoints.points.Value;
            playerPoints.AddPoints();

            Debug.Log("points added");

           StartCoroutine(FlyToPlayerAndDestory(other.gameObject.transform));
        }
    }

    private IEnumerator FlyToPlayerAndDestory(Transform playerTransform)
    {
        float timer = 0;
        float timeToWait = 0.8f;

        while (timer < timeToWait)
        {
            if ( playerTransform == null) break;

            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            timer += Time.deltaTime;   
            
            yield return null;
        }
     
        
        Destroy(gameObject);
    }
}
