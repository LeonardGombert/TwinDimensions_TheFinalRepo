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
    Tilemap generalTilemap;
    [FoldoutGroup("Tilemap")][SerializeField]
    Tile highlightTile;

    [FoldoutGroup("Portal Exits")][SerializeField]
    List<GameObject> portalExits = new List<GameObject>();

    [FoldoutGroup("Checkpoint Teleporter")][SerializeField]
    List <GameObject> hookTower;

    GameObject player;
    GameObject portalEntrance;

    Vector3 entrancePosition;
    Vector3 exitPosition;
    
    Vector3 worldMousePosition;
    Vector3Int currentPortalSelected;
    Vector3Int previousPortalSelected;

    int currentIndexNumber = 0;
    int maxIndexNmber = 0;

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
        if(TeleportationManager.hasTeleported) CheckIfLayerContainsHook(hookTower);
       
        maxIndexNmber = portalExits.Count;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f  && currentIndexNumber <= maxIndexNmber) currentIndexNumber += 1;

        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentIndexNumber > 0) currentIndexNumber -= 1;

        if(currentIndexNumber >= maxIndexNmber) currentIndexNumber = 0;

        if(portalExits.Count != 0) SelectPortalExit();
    }

    private void GetAllPortals(GameObject touchedPortal)
    {
        portalExits.Clear();  

        portalExits.AddRange(GameObject.FindGameObjectsWithTag("Portal"));
        portalExits.Remove(touchedPortal);                               
    }

    void SelectPortalExitWithScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f  && currentIndexNumber < maxIndexNmber) currentIndexNumber += 1;

        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentIndexNumber > 0) currentIndexNumber -= 1;

        if(currentIndexNumber >= maxIndexNmber) currentIndexNumber = 0;
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
