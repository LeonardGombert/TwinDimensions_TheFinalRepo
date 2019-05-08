using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField]
    public List<Sprite> spriteList = new List<Sprite>();

    /*[SerializeField]
    public List<Animation> animations = new List<Animation>();*/

    BoxCollider2D colider;
    SpriteRenderer sr;
    Animation anima;

    void Awake()
    {
        colider = gameObject.GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        /*if(colider.enabled)
        {
            sr.sprite = spriteList[1];
        }
        else
        {
            sr.sprite = spriteList[0];
        }*/

        if(colider.enabled)
        {
            sr.sprite = spriteList[1];
        }
        else
        {
            sr.sprite = spriteList[0];
        }
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

    void Activated()
    {
        if(colider.enabled)
        {sr.sprite = spriteList[0];
        colider.enabled = false;}

        else
        {
        sr.sprite = spriteList[1];
        colider.enabled = true;
        }
    }

    void Released()
    {
        if(colider.enabled)
        {sr.sprite = spriteList[0];
        colider.enabled = false;}

        else
        {
        sr.sprite = spriteList[1];
        colider.enabled = true;
        }
    }
}
