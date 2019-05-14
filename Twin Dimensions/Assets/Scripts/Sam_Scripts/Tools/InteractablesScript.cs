using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesScript : MonoBehaviour

{
    [SerializeField]
    List<Sprite> activationTypeSprite = new List<Sprite>();

    [SerializeField]
    List<GameObject> interactableObjects = new List<GameObject>();


    public enum ActivationType
    {
        Plate, Lever
    }

    public ActivationType activationType;
    public float requiredMass = 1f;
    public float requiredSand = 0f;
    SpriteRenderer sr;

    void Awake ()
    {
        sr = GetComponent<SpriteRenderer>();

        if (activationType == ActivationType.Plate)
        {
            sr.sprite = activationTypeSprite[0];
        }
        else sr.sprite = activationTypeSprite[1];
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {

        if (activationType == ActivationType.Plate && collider.attachedRigidbody.mass >= requiredMass && collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Elephant"))
        {
                foreach (GameObject interactable in interactableObjects)
                {
                    interactable.SendMessage("Activated");
                }

        }

        else if (activationType == ActivationType.Lever && SandTextScript.sandAmount >= requiredSand && collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Elephant"))
        {
            foreach (GameObject interactable in interactableObjects)
                {
                    interactable.SendMessage("Activated");
                }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (activationType == ActivationType.Plate && collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Elephant"))
        {
            foreach (GameObject interactable in interactableObjects)
            {
            interactable.SendMessage("Released");
            }
        }
    }
}