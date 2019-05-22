using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class AttackCollisionDetection : SerializedMonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            PlayerController.isInSlamRange = true;
            Debug.Log("I just rammed the player's ass, yo");
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            PlayerController.isInSlamRange = true;
            Debug.Log("I just rammed the player's ass, yo");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            PlayerController.isInSlamRange = false;
            Debug.Log("I just rammed the player's ass, yo");
        }
    }
}
