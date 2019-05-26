using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class AttackCollisionDetection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            PlayerController.playerIsDead = true;
            Debug.Log("I just rammed the player's ass, yo");
        }

        if(collider.tag == "Elephant")
        {
            Destroy(collider.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            PlayerController.isInSlamRange = true;
            //Debug.Log("I just rammed the player's ass, yo");
        }

        if(collider.tag == "Elephant")
        {
            Destroy(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            PlayerController.isInSlamRange = false;
            //Debug.Log("I just rammed the player's ass, yo");
        }
    }
}
