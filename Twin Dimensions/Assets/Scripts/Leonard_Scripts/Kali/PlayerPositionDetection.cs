using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionDetection : MonoBehaviour
{
    public GameObject Kali;

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            if(KaliBossAI.isTrackingPlayerPosition && !LayerManager.PlayerIsInRealWorld()) Kali.gameObject.SendMessage("UpdatePlayerSide", this.gameObject);
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