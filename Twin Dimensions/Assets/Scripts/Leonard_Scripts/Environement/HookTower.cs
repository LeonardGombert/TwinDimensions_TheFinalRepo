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
            Debug.Log("I hit the player");

            if(PlayerInputManager.instance.GetKey("interaction"))
            {
                Debug.Log("I'm interacting");
                GameObject manager;
                manager = GameObject.FindGameObjectWithTag("Manager");
                manager.SendMessage("GetAllHooks", this.gameObject); 
            }                   
        }
    }
}