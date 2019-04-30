﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class TeleportationManager : SerializedMonoBehaviour
{
    public static TeleportationManager instance;

    GameObject portalEntrance;
    [SerializeField]
    List<GameObject> portalExits = new List<GameObject>();
    [SerializeField]
    List <GameObject> hookTower;

    Vector3 entrancePosition;
    Vector3 exitPosition;

    [SerializeField]
    GameObject player;
    
    Vector3 worldMousePosition;
    Vector3Int currentPortalSelected;
    Vector3Int previousPortalSelected;
    public Tilemap generalTilemap;
    public Tile highlightTile;

    int currentIndexNumber = 0;

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
       
        if (Input.GetAxis("Mouse ScrollWheel") > 0f  && currentIndexNumber <= portalExits.Count) currentIndexNumber += 1;

        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentIndexNumber > 0) currentIndexNumber -= 1;

        if(portalExits.Count != 0) SelectPortalExit();
    }

    private void GetAllPortals(GameObject touchedPortal)
    {
        portalExits.Clear();
        currentIndexNumber = 0;    

        portalExits.AddRange(GameObject.FindGameObjectsWithTag("Portal"));
        portalExits.Remove(touchedPortal);                               
    }

    private void SelectPortalExit()
    {
        currentPortalSelected = generalTilemap.WorldToCell(portalExits[currentIndexNumber].transform.position);

        if (currentPortalSelected != previousPortalSelected)
        {
            generalTilemap.SetTile(currentPortalSelected, highlightTile);

            generalTilemap.SetTile(previousPortalSelected, null);

            previousPortalSelected = currentPortalSelected;
        } 

        if(PlayerInputManager.instance.GetKeyDown("selectedPortalExit")) TeleportToExit(currentIndexNumber);
    }

    private void TeleportToExit(int exitIndexNumber)
    {
        exitPosition = portalExits[exitIndexNumber].transform.position;

        player.transform.position = exitPosition;
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
