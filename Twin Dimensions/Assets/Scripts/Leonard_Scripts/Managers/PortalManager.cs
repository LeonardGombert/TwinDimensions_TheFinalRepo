﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class PortalManager : SerializedMonoBehaviour
{
    public static PortalManager instance;

    [FoldoutGroup("Tilemap")]
    Tilemap movementTilemap;
    [FoldoutGroup("Tilemap")][SerializeField]
    Tile highlightTile;

    [FoldoutGroup("DEBUG Portal Exits")][SerializeField]
    List<GameObject> world1Portals = new List<GameObject>();
    [FoldoutGroup("DEBUG Portal Exits")][SerializeField]
    List<GameObject> world2Portals = new List<GameObject>();
    [FoldoutGroup("DEBUG Portal Exits")][SerializeField]
    List<GameObject> currentWorldPortal = new List<GameObject>();
    
    [FoldoutGroup("Checkpoint Teleporter")][SerializeField]
    GameObject insertBaseActiveTower;
    [FoldoutGroup("Checkpoint Teleporter")][SerializeField]
    List<GameObject> fillWithActiveHookTowers = new List<GameObject>();
    [FoldoutGroup("DEBUG Checkpoint Teleporter")][SerializeField]
    GameObject currentActiveTower;
    [FoldoutGroup("DEBUG Checkpoint Teleporter")][SerializeField]
    List<GameObject> inactiveTowers = new List<GameObject>();
    [FoldoutGroup("DEBUG Checkpoint Teleporter")][SerializeField]
    List <GameObject> hookTowers;

    GameObject player;
    GameObject portalEntrance;

    Vector3 entrancePosition;
    Vector3 exitPosition;
    
    Vector3 worldMousePosition;
    Vector3Int currentPortalSelected;
    Vector3Int previousPortalSelected;

    int currentIndexNumber = 0;
    int maxIndexNmber = 0;
    
    public static bool hasUsedPortal = false;

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
        hookTowers.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower"));
        movementTilemap = GameObject.FindGameObjectWithTag("Movement Tilemap").GetComponent<Tilemap>();
        
        currentActiveTower = insertBaseActiveTower;
        inactiveTowers.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower"));    
        inactiveTowers.Remove(currentActiveTower);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPortalLayer();

        if(TeleportationManager.hasTeleported) CheckIfLayerContainsHook(fillWithActiveHookTowers);
       
        maxIndexNmber = currentWorldPortal.Count;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f  && currentIndexNumber <= maxIndexNmber) currentIndexNumber += 1;

        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentIndexNumber > 0) currentIndexNumber -= 1;

        if(currentIndexNumber >= maxIndexNmber) currentIndexNumber = 0;

        if(currentWorldPortal.Count != 0) SelectPortalExit();
    }

    void CheckForPortalLayer()
    {
        
    }

    private void UpdatePortals(GameObject touchedPortal = default)
    {
        currentWorldPortal.Clear();  

        world1Portals.AddRange(GameObject.FindGameObjectsWithTag("Portal 1"));
        world2Portals.AddRange(GameObject.FindGameObjectsWithTag("Portal 2"));
        
        if(LayerManager.PlayerIsInRealWorld()) currentWorldPortal = world1Portals;
        if(!LayerManager.PlayerIsInRealWorld()) currentWorldPortal = world2Portals;
        
        currentWorldPortal.Remove(touchedPortal);
    }
    
    private void GetAllHooks (GameObject newHookTower)
    {
        currentActiveTower = newHookTower;

        inactiveTowers.Clear();
        fillWithActiveHookTowers.Clear();

        inactiveTowers.AddRange(GameObject.FindGameObjectsWithTag("Inactive Hook Tower"));
        inactiveTowers.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower"));
        inactiveTowers.Remove(currentActiveTower);

        fillWithActiveHookTowers.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower"));

        foreach (GameObject tower in inactiveTowers)
        {
            tower.tag = "Inactive Hook Tower";
        }
    }

    private void SelectPortalExit()
    {
        currentPortalSelected = movementTilemap.WorldToCell(currentWorldPortal[currentIndexNumber].transform.position);

        if (currentPortalSelected != previousPortalSelected)
        {
            movementTilemap.SetTile(currentPortalSelected, highlightTile);

            movementTilemap.SetTile(previousPortalSelected, null);

            previousPortalSelected = currentPortalSelected;
        } 

        if(PlayerInputManager.instance.GetKeyDown("selectedPortalExit"))
        {
            movementTilemap.SetTile(currentPortalSelected, null);
            TeleportToExit(currentIndexNumber);
        }
    }

    private void TeleportToExit(int exitIndexNumber)
    {
        hasUsedPortal = true;
        exitPosition = currentWorldPortal[exitIndexNumber].transform.position;
        player.transform.position = exitPosition;
        PlayerController.canMove = true;
    }

    private void PlayerLeftPortalRange()
    {
        currentWorldPortal.Clear();
        movementTilemap.SetTile(currentPortalSelected, null);
    }

    private void CheckIfLayerContainsHook(List<GameObject> hookTowerList)
    {
        foreach (GameObject hookTower in hookTowerList)
        {
            Vector3 hookTowerPosition = hookTower.transform.position;

            if(hookTower.gameObject.layer == LayerMask.NameToLayer("Hook Layer 1")
            && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 1"))
            {
                TeleportToHook(hookTowerPosition);
            }

            if(hookTower.gameObject.layer == LayerMask.NameToLayer("Hook Layer 2")
            && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 2"))
            {
                TeleportToHook(hookTowerPosition);
            }            
        }
    }   

    private void TeleportToHook(Vector3 towerPosition)
    {
        player.transform.position = towerPosition;        
        TeleportationManager.hasTeleported = false;
    }
}