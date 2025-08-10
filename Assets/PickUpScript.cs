using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{   
    public GameObject ItemOnPlayer;
    
    void Start()
    {
        ItemOnPlayer.SetActive(false);
        
    }

   private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
           if (Input.GetKey(KeyCode.E))
           {
              this.gameObject.SetActive(false);

              ItemOnPlayer.SetActive(true);
           }
        }
    }
}