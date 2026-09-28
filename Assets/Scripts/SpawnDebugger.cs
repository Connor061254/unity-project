using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("object spawned!" + System.Environment.StackTrace);
    }
}
