using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    BoxCollider2D colider;
    SpriteRenderer sprite;

    void Awake()
    {
        colider = gameObject.GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    /*void OnEnable()
    {
        EventManager.StartListening("InteractableActivated", OpenDoor);
        EventManager.StartListening("InteractableReleased", CloseDoor);
    }

    void OnDisable()
    {
        EventManager.StopListening("InteractableActivated", OpenDoor);
        EventManager.StopListening("InteractableReleased", CloseDoor);
    }*/

    void OpenDoor()
    {
        if(colider.enabled)
        {sprite.color = Color.blue;
        colider.enabled = false;}

        else
        {
        sprite.color = Color.red;
        colider.enabled = true;
        }
    }

    void CloseDoor()
    {
        if(colider.enabled)
        {sprite.color = Color.blue;
        colider.enabled = false;}

        else
        {
        sprite.color = Color.red;
        colider.enabled = true;
        }
    }
}
