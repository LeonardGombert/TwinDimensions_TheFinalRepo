using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class PortalManager : MonoBehaviour
{
    [FoldoutGroup("Tilemap")]
    Tilemap movementTilemap;
    [FoldoutGroup("Tilemap")][SerializeField]
    Tile highlightTile;
    [FoldoutGroup("Checkpoint Teleporter")][SerializeField]
    GameObject insertBaseActiveTower;
    [FoldoutGroup("Insert Locked Portals")][SerializeField]
    List<GameObject> lockedPortals = new List<GameObject>();

    [FoldoutGroup("DEBUG Portal Exits")][SerializeField]
    List<GameObject> world1Portals = new List<GameObject>();
    [FoldoutGroup("DEBUG Portal Exits")][SerializeField]
    List<GameObject> world2Portals = new List<GameObject>();
    [FoldoutGroup("DEBUG Portal Exits")][SerializeField]
    List<GameObject> currentWorldPortal = new List<GameObject>();
    
    [FoldoutGroup("DEBUG Checkpoint Teleporter")][SerializeField]
    GameObject currentActiveTower;
    [FoldoutGroup("DEBUG Checkpoint Teleporter")][SerializeField]
    List<GameObject> activeHookTowersWorld1 = new List<GameObject>();
    [FoldoutGroup("DEBUG Checkpoint Teleporter")][SerializeField]
    List<GameObject> inactiveHookTowersWorld1 = new List<GameObject>();
    [FoldoutGroup("DEBUG Checkpoint Teleporter")][SerializeField]
    List<GameObject> activeHookTowersWorld2 = new List<GameObject>();
    [FoldoutGroup("DEBUG Checkpoint Teleporter")][SerializeField]
    List<GameObject> inactiveHookTowersWorld2 = new List<GameObject>();

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
        player = GameObject.FindGameObjectWithTag("Player");
        movementTilemap = GameObject.FindGameObjectWithTag("Movement Tilemap").GetComponent<Tilemap>();
        
        currentActiveTower = insertBaseActiveTower;

        inactiveHookTowersWorld1.AddRange(GameObject.FindGameObjectsWithTag("Inactive Hook Tower 1"));
        inactiveHookTowersWorld2.AddRange(GameObject.FindGameObjectsWithTag("Inactive Hook Tower 2"));

        activeHookTowersWorld1.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower 1"));
        activeHookTowersWorld2.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower 2"));

        //inactiveHookTowersWorld1.Remove(currentActiveTower);
        //inactiveHookTowersWorld1.Remove(currentActiveTower);
        
        foreach(GameObject portal in lockedPortals)
        {
            portal.gameObject.SendMessage("LockPortals");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(TeleportationManager.hasTeleported) CheckIfLayerContainsHook(activeHookTowersWorld1);
        if(TeleportationManager.hasTeleported) CheckIfLayerContainsHook(activeHookTowersWorld2);
       
        maxIndexNmber = currentWorldPortal.Count;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f  && currentIndexNumber <= maxIndexNmber) currentIndexNumber += 1;

        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentIndexNumber > 0) currentIndexNumber -= 1;

        if(currentIndexNumber >= maxIndexNmber) currentIndexNumber = 0;

        if(currentWorldPortal.Count != 0) SelectPortalExit();
    }

    #region //PORTALS
    private void UpdatePortals(GameObject touchedPortal = default)
    {
        currentWorldPortal.Clear();

        world1Portals.AddRange(GameObject.FindGameObjectsWithTag("Portal 1"));
        world2Portals.AddRange(GameObject.FindGameObjectsWithTag("Portal 2"));

        foreach (GameObject lockedP in lockedPortals)
        {
            world1Portals.Remove(lockedP);
            world2Portals.Remove(lockedP);
        }
        
        if(LayerManager.PlayerIsInRealWorld()) currentWorldPortal = world1Portals;
        if(!LayerManager.PlayerIsInRealWorld()) currentWorldPortal = world2Portals;
        
        currentWorldPortal.Remove(touchedPortal);
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

    void UnlockThisPortal(GameObject portalToRemove)
    {
        lockedPortals.Remove(portalToRemove);
    }
    #endregion
    
    #region //TELEPORTER
    private void GetAllHooks(GameObject newHookTower)
    {
        if(newHookTower.layer == LayerMask.NameToLayer("Hook Layer 1")) newHookTower.tag = "Hook Tower 1";
        if(newHookTower.layer == LayerMask.NameToLayer("Hook Layer 2")) newHookTower.tag = "Hook Tower 2";

        if(newHookTower.layer == LayerMask.NameToLayer("Hook Layer 1"))
        {
            foreach (GameObject tower in activeHookTowersWorld1)
            {
                if(tower.layer == LayerMask.NameToLayer("Hook Layer 1"))
                {
                    tower.tag = "Inactive Hook Tower 1";
                    activeHookTowersWorld1.Clear();
                    activeHookTowersWorld1.Add(newHookTower);
                    inactiveHookTowersWorld1.Add(tower);
                    inactiveHookTowersWorld1.Remove(newHookTower);
                }
            }
        }

        if (newHookTower.layer == LayerMask.NameToLayer("Hook Layer 2"))
        {
            foreach (GameObject tower in activeHookTowersWorld2)
            {
                if (tower.layer == LayerMask.NameToLayer("Hook Layer 2"))
                {
                    tower.tag = "Inactive Hook Tower 2";
                    activeHookTowersWorld2.Clear();
                    activeHookTowersWorld2.Add(newHookTower);
                    inactiveHookTowersWorld2.Add(tower);
                    inactiveHookTowersWorld2.Remove(newHookTower);
                }
            }
        }
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
        player.transform.position = new Vector3(towerPosition.x, towerPosition.y + .5f); //offsets player positing correctly 
        TeleportationManager.hasTeleported = false;
    }
    #endregion
}