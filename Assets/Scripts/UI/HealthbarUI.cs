using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
   public Slider myCanvasSlider; 

   public void SetMaxHealth(float health)
    {
        myCanvasSlider.maxValue = health;
        myCanvasSlider.value = health;
    }

    public void SetHealth(float health)
    {
        myCanvasSlider.value = health;
    }
}
