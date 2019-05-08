using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HookTower : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(PlayerInputManager.instance.GetKey("interaction"))
            {
                GameObject manager;
                manager = GameObject.FindGameObjectWithTag("Manager");
                manager.SendMessage("GetAllHooks", this.gameObject); 
            }                   
        }
    }
}