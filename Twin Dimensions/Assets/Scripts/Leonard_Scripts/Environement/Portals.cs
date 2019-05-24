using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Portals : MonoBehaviour
{
    private bool thisPortalIsLocked = false;  
    SpriteRenderer sr;
    Sprite lockedSprite;
    Sprite unlockedSprite;
    Animator anim;
    GameObject manager;
    bool isInteracting = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        manager = GameObject.FindGameObjectWithTag("Manager");
    }

    void Update()
    {
        if(PlayerInputManager.instance.GetKey("interactionKey")) isInteracting = true;

        if (PlayerInputManager.instance.GetKeyUp("interactionKey"))
        {
            isInteracting = false;
            PlayerController.canMove = true;
            PortalManager.hasUsedPortal = false;
            manager.SendMessage("PlayerLeftPortalRange");
        }
    }    

    bool PlayerHasRequiredSandAmount()
    {
        if(SandManager.playerSandAmount >= 1)
        {
            SandManager.playerSandAmount-- ;
            return true;
        }
        else return false;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(isInteracting == true)
        {
            if(collider.gameObject.tag == "Player" && PortalManager.hasUsedPortal == false)
            {
                if (thisPortalIsLocked)
                {
                    if (PlayerHasRequiredSandAmount())
                    {
                        anim.SetBool("unlockPortal", true);
                        new WaitForEndOfFrame();
                        sr.sprite = unlockedSprite;

                        manager.SendMessage("UpdatePortals", this.gameObject);
                        PortalManager.hasUsedPortal = true;
                        PlayerController.canMove = false;
                    }

                    else return;
                }

                else if (!thisPortalIsLocked)
                {
                    anim.SetBool("unlockPortal", true);
                    new WaitForEndOfFrame();
                    sr.sprite = unlockedSprite;

                    manager.SendMessage("UpdatePortals", this.gameObject);
                    PortalManager.hasUsedPortal = true;
                    PlayerController.canMove = false;
                }                    
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(isInteracting == true)
        {
            if(collider.gameObject.tag == "Player" && PortalManager.hasUsedPortal == true)
            {
                if (thisPortalIsLocked)
                {
                    if (PlayerHasRequiredSandAmount())
                    {
                        manager.SendMessage("UpdatePortals", this.gameObject);
                        PortalManager.hasUsedPortal = true;
                        PlayerController.canMove = false;
                        thisPortalIsLocked = false;
                    }

                    else return;
                }

                else if (!thisPortalIsLocked)
                {
                    manager.SendMessage("UpdatePortals", this.gameObject);
                    PortalManager.hasUsedPortal = true;
                    PlayerController.canMove = false;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        isInteracting = false;
    }

    void LockPortals()
    {
        thisPortalIsLocked = true;
        sr.sprite = lockedSprite;
    }

    void UnlockPortals()
    {

    }
}