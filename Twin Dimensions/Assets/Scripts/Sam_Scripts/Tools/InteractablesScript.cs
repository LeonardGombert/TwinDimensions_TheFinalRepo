using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesScript : MonoBehaviour

{

    [SerializeField]
    public class CaughtObject
    {
        public Rigidbody2D rigidbody;
        public Collider2D collider;
        public bool inContact;
    }

    [SerializeField]
    List<GameObject> doorObjects = new List<GameObject>();


    public enum ActivationType
    {
        Plate, Lever
    }

    public ActivationType activationType;
    public float requiredMass = 1f;
    public float requiredSand = 0f;

    //public BoxCollider2D boxCollider;

    public void OnTriggerStay2D(Collider2D collider)
    {

        if (activationType == ActivationType.Plate && collider.attachedRigidbody.mass >= requiredMass)
        {
            //if (collider.attachedRigidbody.mass >= requiredMass)
            //{
                foreach (GameObject door in doorObjects)
                {
                    EventManager.TriggerEvent("InteractableActivated");
                }
            //}

        }

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (activationType == ActivationType.Lever && SandTextScript.sandAmount >= requiredSand)
        {
                EventManager.TriggerEvent("InteractableActivated");
                SandTextScript.sandAmount -= (int)requiredSand;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (activationType == ActivationType.Plate)
        {
            Debug.Log("Pressure Plate released");
            EventManager.TriggerEvent("InteractableReleased");
        }
    }

}