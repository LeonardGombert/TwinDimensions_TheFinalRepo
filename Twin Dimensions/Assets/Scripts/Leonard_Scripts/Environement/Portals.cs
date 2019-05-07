using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Portals : MonoBehaviour
{
    bool secondPortalUseAvailable = false;
    
    GameObject manager;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
    }

    void Update()
    {
        if(PlayerInputManager.instance.GetKeyDown("back"))
        {
            PlayerController.canMove = true;
            PortalManager.hasUsedPortal = false;
            manager.SendMessage("PlayerLeftPortalRange");
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player"&& PortalManager.hasUsedPortal == false)
        {
            if(PlayerInputManager.instance.GetKeyDown("interaction"))
            {
                manager.SendMessage("UpdatePortals", this.gameObject);
                PortalManager.hasUsedPortal = true;
                PlayerController.canMove = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player" && PortalManager.hasUsedPortal == true)
        {
            manager.SendMessage("UpdatePortals", this.gameObject);
            PlayerController.canMove = false;
        }
    }
}