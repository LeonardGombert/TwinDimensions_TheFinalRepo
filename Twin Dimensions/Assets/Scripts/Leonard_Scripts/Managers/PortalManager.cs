using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class PortalManager : SerializedMonoBehaviour
{
    public static PortalManager instance;

    [FoldoutGroup("Tilemap")][SerializeField]
    Tilemap movementTilemap;
    [FoldoutGroup("Tilemap")][SerializeField]
    Tile highlightTile;

    [FoldoutGroup("Portal Exits")][SerializeField]
    List<GameObject> portalExits = new List<GameObject>();

    [FoldoutGroup("Checkpoint Teleporter")]
    List <GameObject> hookTower;
    [FoldoutGroup("Checkpoint Teleporter")][SerializeField]
    GameObject baseActiveTower;
    [FoldoutGroup("Checkpoint Teleporter")]
    GameObject currentActiveTower;
    [FoldoutGroup("Checkpoint Teleporter")][SerializeField]
    List<GameObject> activeHookTowers = new List<GameObject>();
    [FoldoutGroup("Checkpoint Teleporter")]
    List<GameObject> inactiveTowers = new List<GameObject>();

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
        hookTower.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower"));
        movementTilemap = GameObject.FindGameObjectWithTag("Movement Tilemap").GetComponent<Tilemap>();
        
        currentActiveTower = baseActiveTower;
        inactiveTowers.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower"));    
        inactiveTowers.Remove(currentActiveTower);
    }

    // Update is called once per frame
    void Update()
    {
        if(TeleportationManager.hasTeleported) CheckIfLayerContainsHook(activeHookTowers);
       
        maxIndexNmber = portalExits.Count;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f  && currentIndexNumber <= maxIndexNmber) currentIndexNumber += 1;

        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentIndexNumber > 0) currentIndexNumber -= 1;

        if(currentIndexNumber >= maxIndexNmber) currentIndexNumber = 0;

        if(portalExits.Count != 0) SelectPortalExit();
    }

    private void UpdatePortals(GameObject touchedPortal = default)
    {
        portalExits.Clear();  

        portalExits.AddRange(GameObject.FindGameObjectsWithTag("Portal"));
        portalExits.Remove(touchedPortal);
    }
    
    private void GetAllHooks (GameObject newHookTower)
    {
        currentActiveTower = newHookTower;

        inactiveTowers.Clear();
        activeHookTowers.Clear();

        inactiveTowers.AddRange(GameObject.FindGameObjectsWithTag("Inactive Hook Tower"));
        inactiveTowers.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower"));
        inactiveTowers.Remove(currentActiveTower);

        activeHookTowers.AddRange(GameObject.FindGameObjectsWithTag("Hook Tower"));

        foreach (GameObject tower in inactiveTowers)
        {
            tower.tag = "Inactive Hook Tower";
        }
    }

    private void SelectPortalExit()
    {
        currentPortalSelected = movementTilemap.WorldToCell(portalExits[currentIndexNumber].transform.position);

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
        exitPosition = portalExits[exitIndexNumber].transform.position;
        player.transform.position = exitPosition;
        PlayerController.canMove = true;
    }

    private void PlayerLeftPortalRange()
    {
        portalExits.Clear();
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