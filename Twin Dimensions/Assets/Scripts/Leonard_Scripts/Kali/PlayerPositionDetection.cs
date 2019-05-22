using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionDetection : MonoBehaviour
{
    public GameObject Kali;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            if(KaliBossAI.isTrackingPlayerPosition) Kali.gameObject.SendMessage("UpdatePlayerSide", this.gameObject);
            else return;            
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            if(KaliBossAI.isTrackingPlayerPosition) Kali.gameObject.SendMessage("UpdatePlayerSide", this.gameObject);
            else return; 
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            PlayerController.isInSlamRange = false;
        }
    }

    void Slamming()
    {
        if(PlayerController.isInSlamRange == true) PlayerController.playerIsDead = true;//Debug.Log(this.gameObject.name + " just smashed the player");

        if(PlayerController.isInSlamRange == false) Debug.Log("I missed");
    }

    void Sweeping()
    {
        if(PlayerController.isInSlamRange == true) PlayerController.playerIsDead = true;//Debug.Log(this.gameObject.name + " just smashed the player");

        if(PlayerController.isInSlamRange == false) Debug.Log("I missed");
    }
}