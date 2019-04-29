using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class TeleportationManager : SerializedMonoBehaviour
{
    public static TeleportationManager instance;

    GameObject portalEntrance;
    [SerializeField]
    List<GameObject> portalExits;
    [SerializeField]
    List <GameObject> hookTower;

    Vector3 entrancePosition;
    Vector3 exitPosition;

    [SerializeField]
    GameObject player;
     
    void Awake()
    {
        if(instance == null)
        {            
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        player = GameObject.FindGameObjectWithTag("Player");
        hookTower.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower"));
    }


    // Update is called once per frame
    void Update()
    {
        if(Teleportation.hasTeleported) CheckIfLayerContainsHook(hookTower);
    }

    public void PlayerHasEntered(GameObject touchedPortal)
    {
        Debug.Log("I've received the gameObject " + touchedPortal);

        touchedPortal = portalEntrance;

        portalExits.AddRange(GameObject.FindGameObjectsWithTag("Portals"));
        portalExits.Remove(touchedPortal);
    }

    public void FirstPortal(GameObject firstPortal)
    {
        player.transform.position = firstPortal.transform.position;
    }

    private void CheckIfLayerContainsHook(List<GameObject> hookTowerList)
    {
        foreach (GameObject hookTower in hookTowerList)
        {
            Vector3 hookTowerPosition = hookTower.transform.position;

            if(hookTower.gameObject.layer == LayerMask.NameToLayer("Hook Layer 1")
            && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 1"))
            {
                player.SendMessage("TeleportToHook", hookTowerPosition);
            }

            if(hookTower.gameObject.layer == LayerMask.NameToLayer("Hook Layer 2")
            && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 2"))
            {
                player.SendMessage("TeleportToHook", hookTowerPosition);
            }            
        }
    }
}
