using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Portals : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            GameObject manager;
            manager = GameObject.FindGameObjectWithTag("Manager");
            manager.SendMessage("GetAllPortals", this.gameObject);        
        }
    }
}