using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Portals : MonoBehaviour
{
    public GameObject portalEntrance;
    public GameObject portalExit;
    bool isOnPortal = false;
    GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

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
