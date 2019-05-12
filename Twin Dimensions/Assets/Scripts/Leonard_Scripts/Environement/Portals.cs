using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Portals : MonoBehaviour
{    
    GameObject manager;
    bool isInteracting = false;

    void Awake()
    {

    }

    void Update()
    {
        if(PlayerInputManager.instance.GetKey("interaction"))
        { 
            isInteracting = true;
        }
        else 
        {
            isInteracting = false;
            PlayerController.canMove = true;
            PortalManager.hasUsedPortal = false;
            manager.SendMessage("PlayerLeftPortalRange");
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(isInteracting == true)
        {
            if(collider.gameObject.tag == "Player" && PortalManager.hasUsedPortal == false)
            {
                manager.SendMessage("UpdatePortals", this.gameObject);
                PortalManager.hasUsedPortal = true;
                PlayerController.canMove = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(isInteracting == true)
        {
            if(collider.gameObject.tag == "Player" && PortalManager.hasUsedPortal == true)
            {
                manager.SendMessage("UpdatePortals", this.gameObject);
                PlayerController.canMove = false;
            }
        }
    }
}