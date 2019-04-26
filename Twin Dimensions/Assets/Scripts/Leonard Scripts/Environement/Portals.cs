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

    // Update is called once per frame
    void Update()
    {
        
    }

    void TeleportToExit()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            portalEntrance = this.gameObject;
            isOnPortal = true;
            player.SendMessage("TeleportToPortalExit");
        }
    }
}
