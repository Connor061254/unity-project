using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public GameObject popUpCanvas;
  public void ShowPrompt()
    {
        if (popUpCanvas != null)
        {
            popUpCanvas.SetActive(true);
        }
    }

    public void HidePrompt()
    {
        if (popUpCanvas != null)
        {
            popUpCanvas.SetActive(false);
        }
    }
}
