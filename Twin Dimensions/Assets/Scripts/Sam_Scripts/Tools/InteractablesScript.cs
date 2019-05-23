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
        Plate, Lever, Gong, Receptacle
    }

    public ActivationType activationType;
    public float requiredMass = 1f;
    public float requiredSand = 0f;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (activationType == ActivationType.Plate)
        {
            sr.sprite = activationTypeSprite[0];
        }
        else if (activationType == ActivationType.Lever)
        {
            sr.sprite = activationTypeSprite[1];
        }
        else if (activationType == ActivationType.Gong)
        {
            sr.sprite = activationTypeSprite[2];
        }
        else if (activationType == ActivationType.Receptacle)
        {
            sr.sprite = activationTypeSprite[3];
        }
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (activationType == ActivationType.Plate && collider.attachedRigidbody.mass >= requiredMass && collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Elephant"))
        {
            foreach (GameObject interactable in interactableObjects)
            {
                GUICameraController.MoveCameraToPosition(interactable.transform.position, interactable.gameObject.layer);
                if (PlayerInputManager.instance.GetKeyDown("interactionKey")) interactable.SendMessage("Activated");
                break;
            }
        }

        else if (activationType == ActivationType.Lever && collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Elephant"))
        {
            foreach (GameObject interactable in interactableObjects)
            {
                GUICameraController.MoveCameraToPosition(interactable.transform.position, interactable.gameObject.layer);
                if (PlayerInputManager.instance.GetKeyDown("interactionKey")) interactable.SendMessage("Activated");
                break;
            }
        }

        else if (activationType == ActivationType.Gong && collider.gameObject.CompareTag("Elephant"))
        {
            foreach (GameObject interactable in interactableObjects)
            {
                GUICameraController.MoveCameraToPosition(interactable.transform.position, interactable.gameObject.layer);
                interactable.SendMessage("Activated");
            }
        }

        else if (activationType == ActivationType.Receptacle && SandManager.mySandAmount >= requiredSand && collider.gameObject.CompareTag("Player"))
        {
            foreach (GameObject interactable in interactableObjects)
            {
                GUICameraController.MoveCameraToPosition(interactable.transform.position, interactable.gameObject.layer);
                if (PlayerInputManager.instance.GetKeyDown("interactionKey")) interactable.SendMessage("Activated");
                break;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (activationType == ActivationType.Plate)
        {
            foreach (GameObject interactable in interactableObjects)
            {
                interactable.SendMessage("Released");
            }
        }
    }
}