using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Portals : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite lockedSprite;
    [SerializeField] Animator anim;
    GameObject manager;
    [SerializeField] bool isInteracting = false;
    [SerializeField] bool thisPortalIsLocked = false;  

    void Start()
    {
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
        if (isInteracting == true)
        {
            if (collider.gameObject.tag == "Player" && PortalManager.hasUsedPortal == false)
            {
                if (thisPortalIsLocked)
                {
                    if (PlayerHasRequiredSandAmount())
                    {
                        anim.enabled = true;
                        anim.SetBool("isUnlocked", true);

                        manager.SendMessage("UpdatePortals", this.gameObject);
                        PortalManager.hasUsedPortal = true;
                        PlayerController.canMove = false;
                        UnlockPortal();
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
        anim.enabled = false;
        sr.sprite = lockedSprite;
    }

    void UnlockPortal()
    {
        thisPortalIsLocked = false;
        manager.gameObject.SendMessage("UnlockThisPortal", this.gameObject);
    }
}