using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaliDoor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameObject manager;
            manager = GameObject.FindGameObjectWithTag("Manager");
            manager.gameObject.SendMessage("ReachedExit");               
        }
    }
}
