using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    void Awake()
    {
        Debug.Log("The UI Script has officially started!");
        serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        hostButton.onClick.AddListener(() =>
        {
            Debug.Log("The Host button was successfully clicked!");
            NetworkManager.Singleton.StartHost();
        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });

    }

    void Update()
{
    // If we press the 'H' key on the keyboard...
    if (Input.GetKeyDown(KeyCode.H))
    {
        Debug.Log("Keyboard Bypass: Starting Host!");
        NetworkManager.Singleton.StartHost();
    }
}
}
