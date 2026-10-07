using UnityEngine;

public class PersistentNetworkmanager : MonoBehaviour
{
    private static PersistentNetworkmanager instance;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
