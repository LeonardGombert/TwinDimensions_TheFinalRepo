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

    List<CaughtObject> caughtObjects;


    public enum ActivationType
    {
        Plate, Button
    }

    public ActivationType activationType;
    public float requiredMass = 1f;
    public float requiredSand = 1f;
    public Sprite activatedPadSprite;
    public Sprite releasedPadSprite;
    //public BoxCollider2D boxCollider;

    public void OnTriggerStay2D(Collider2D collider)
    {

        if (activationType == ActivationType.Plate)
        {
            if (collider.attachedRigidbody.mass >= requiredMass)
            {
                    EventManager.TriggerEvent("InteractableActivated");
            }

        }

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(SandTextScript.sandAmount);
        if (activationType == ActivationType.Button && SandTextScript.sandAmount >= requiredSand)
        {
            //if (Input.GetKeyDown("e") && sandNumber >= requiredSand)
            //{
                EventManager.TriggerEvent("InteractableActivated");
                SandTextScript.sandAmount -= (int)requiredSand;
                
            //}
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