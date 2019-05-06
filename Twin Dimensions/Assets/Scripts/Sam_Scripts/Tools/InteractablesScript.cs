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

    public void OnTriggerEnter2D(Collider2D collider)
    {

        if (activationType == ActivationType.Plate && collider.attachedRigidbody.mass >= requiredMass)
        {
                foreach (GameObject door in doorObjects)
                {
                    door.SendMessage("Activated");
                }

        }

        if (activationType == ActivationType.Lever && SandTextScript.sandAmount >= requiredSand)
        {
            foreach (GameObject door in doorObjects)
                {
                    door.SendMessage("Activated");
                }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (activationType == ActivationType.Plate)
        {
            foreach (GameObject door in doorObjects)
            {
            Debug.Log("Pressure Plate released");
            door.SendMessage("Released");
            }
        }
    }

        /*public void OnTriggerEnter2D(Collider2D collider)
    {
        if (activationType == ActivationType.Lever && SandTextScript.sandAmount >= requiredSand)
        {
                EventManager.TriggerEvent("InteractableActivated");
        }
    }*/
}