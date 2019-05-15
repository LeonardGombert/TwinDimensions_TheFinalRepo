using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttack : MonoBehaviour
{
    public GameObject Kali;

    bool isSlamming = false;
    bool hasSentMessage = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(PlayerController.isInSlamRange);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            PlayerController.isInSlamRange = true;

            if(KaliBossAI.isTrackingPlayerSide)
            {
                Kali.gameObject.SendMessage("SlamOnPlayerSide", this.gameObject);
            }
            else return;            
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            PlayerController.isInSlamRange = true;

            if(KaliBossAI.isTrackingPlayerSide)
            {
                Kali.gameObject.SendMessage("SlamOnPlayerSide", this.gameObject);
            }
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
        if(PlayerController.isInSlamRange == true) Debug.Log(this.gameObject.name + " just smashed the player");

        if(PlayerController.isInSlamRange == false) Debug.Log("I missed");
    }

    void Sweeping()
    {

    }
}