using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class PortalManager : SerializedMonoBehaviour
{
    public static PortalManager instance;

    GameObject portalEntrance;
    [SerializeField]
    List<GameObject> portalExits;

    Vector3 entrancePosition;
    Vector3 exitPosition;
     
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
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerHasEntered(GameObject touchedPortal)
    {
        Debug.Log("I've received the gameObject " + touchedPortal);

        touchedPortal = portalEntrance;

        portalExits.AddRange(GameObject.FindGameObjectsWithTag("Portals"));
        portalExits.Remove(touchedPortal);
    }
}
