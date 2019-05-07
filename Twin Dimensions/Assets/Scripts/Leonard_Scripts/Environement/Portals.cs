using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Portals : MonoBehaviour
{
    public static bool hasUsedPortal = false;

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(PlayerInputManager.instance.GetKeyDown("interaction"))
            {
                hasUsedPortal = true;
                GameObject manager;
                manager = GameObject.FindGameObjectWithTag("Manager");
                manager.SendMessage("UpdatePortals", this.gameObject);
                PlayerController.canMove = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(hasUsedPortal == true)
            {
                Debug.Log("yes");
                GameObject manager;
                manager = GameObject.FindGameObjectWithTag("Manager");
                manager.SendMessage("UpdatePortals", this.gameObject);
                PlayerController.canMove = false;
            }
        }
    }
}