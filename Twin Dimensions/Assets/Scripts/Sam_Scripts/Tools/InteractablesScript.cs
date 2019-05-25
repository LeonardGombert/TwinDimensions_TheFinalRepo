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
    public bool isOpen;
    SpriteRenderer sr;
    BoxCollider2D bxc;
    

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        bxc = GetComponent<BoxCollider2D>();

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
            bxc.size = new Vector2 (2, 0.5f);
            bxc.offset = new Vector2 (0, -1);
        }
        else if (activationType == ActivationType.Receptacle)
        {
            sr.sprite = activationTypeSprite[3];
            bxc.size = new Vector2 (3, 2);
            bxc.offset = new Vector2 (0, -1);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (activationType == ActivationType.Plate && collider.gameObject.CompareTag("Elephant") && collider.attachedRigidbody.mass > requiredMass)
        {
            foreach (GameObject interactable in interactableObjects)
            {
                interactable.SendMessage("Released");
            }
        }

        else if (activationType == ActivationType.Gong && collider.gameObject.CompareTag("Elephant"))
        {
            foreach (GameObject interactable in interactableObjects)
            {
                GUICameraController.MoveCameraToPosition(interactable, interactable.gameObject.layer);
                interactable.SendMessage("Activated");
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (activationType == ActivationType.Plate && collider.gameObject.CompareTag("Player") && collider.attachedRigidbody.mass <= requiredMass)
        {
            foreach (GameObject interactable in interactableObjects)
            {
                GUICameraController.MoveCameraToPosition(interactable, interactable.gameObject.layer);
                if (PlayerInputManager.instance.GetKey("interactionKey") && isOpen == false)
                {
                    foreach (GameObject thing in interactableObjects)
                    {
                        isOpen = true;
                        thing.SendMessage("Activated");
                    }
                }
                if (PlayerInputManager.instance.GetKeyUp("interactionKey") && isOpen == true)
                {
                    foreach (GameObject thing2 in interactableObjects)
                    {
                        isOpen = false;
                        thing2.SendMessage("Released");
                    }

                }
                break;
            }
        }

        /*if (activationType == ActivationType.Plate && collider.gameObject.CompareTag("Elephant"))
        {
            foreach (GameObject interactable in interactableObjects)
            {
                interactable.SendMessage("Released");
            }
        }*/

        else if (activationType == ActivationType.Lever && collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Elephant"))
        {
            foreach (GameObject interactable in interactableObjects)
            {
                GUICameraController.MoveCameraToPosition(interactable, interactable.gameObject.layer);
                if (PlayerInputManager.instance.GetKeyDown("interactionKey"))
                {
                    foreach (GameObject thing in interactableObjects)
                    {
                        thing.SendMessage("Activated");
                    }
                }
                break;
            }
        }

        else if (activationType == ActivationType.Receptacle && PlayerController.playerSandAmount >= requiredSand && collider.gameObject.CompareTag("Player"))
        {
            foreach (GameObject interactable in interactableObjects)
            {
                GUICameraController.MoveCameraToPosition(interactable, interactable.gameObject.layer);
                if (PlayerInputManager.instance.GetKeyDown("interactionKey"))
                {
                    foreach (GameObject thing in interactableObjects)
                    {
                        thing.SendMessage("Activated");
                    }
                }
                break;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (activationType == ActivationType.Plate && collider.gameObject.CompareTag("Elephant"))
        {
            foreach (GameObject interactable in interactableObjects)
            {
                interactable.SendMessage("Released");
            }
        }

        GUICameraController.ClearCameraPosition();
    }
}