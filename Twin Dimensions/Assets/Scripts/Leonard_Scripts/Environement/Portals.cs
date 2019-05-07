using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Portals : MonoBehaviour
{
    public static bool hasInteracted = false;

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(PlayerInputManager.instance.GetKeyDown("interaction"))
            {
                GameObject manager;
                manager = GameObject.FindGameObjectWithTag("Manager");
                if(!hasInteracted) manager.SendMessage("GetAllPortals", this.gameObject);
            }
        }
    }
}