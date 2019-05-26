using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HookTower : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (this.gameObject.tag == "Inactive Hook Tower 1")
            {
                if (PlayerInputManager.instance.GetKey("interactionKey"))
                {
                    GameObject manager;
                    manager = GameObject.FindGameObjectWithTag("Manager");
                    manager.SendMessage("GetAllHooks", this.gameObject);
                }
            }

            if (this.gameObject.tag == "Inactive Hook Tower 2")
            {
                if (PlayerInputManager.instance.GetKey("interactionKey"))
                {
                    GameObject manager;
                    manager = GameObject.FindGameObjectWithTag("Manager");
                    manager.SendMessage("GetAllHooks", this.gameObject);
                }
            }
        }
    }
}