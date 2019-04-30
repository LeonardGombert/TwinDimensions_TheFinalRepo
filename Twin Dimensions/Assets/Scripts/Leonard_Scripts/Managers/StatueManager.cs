using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class StatueManager : SerializedMonoBehaviour
{
    public static StatueManager instance;

    public Tilemap generalTilemap;
    public Tile highlightTile;

    Vector3 worldMousePosition;

    Vector3Int currentMousePositionInGrid;
    Vector3Int previous;
    
    public GameObject player;
    public GameObject world1Statue;
    public GameObject world2Statue;

    public static float statueKickSpeed = 800;

    public bool isPlacingStatue = false;
    public bool isSelectingStatueLocation = false;
    public static bool isKickingStatue = false;
    public static bool isInRange = false;

    #region Kick

    public Transform playerPosition;
    Vector3 currentPositionOnGrid;
    Vector3 kickDirection;
    Rigidbody2D rb;

    #endregion

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

        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        SelectStatuePlacement();

        //Check current player layer
        if(LayerManager.PlayerIsInRealWorld()) PlaceStatue(LayerMask.NameToLayer("Player Layer 1"));

        if(!LayerManager.PlayerIsInRealWorld()) PlaceStatue(LayerMask.NameToLayer("Player Layer 2"));

        //Check if player is placing or kicking statues
        if(PlayerInputManager.instance.GetKeyDown("placeStatue")) isSelectingStatueLocation = true;
       
        if(PlayerInputManager.instance.GetKeyDown("kickStatue")) isKickingStatue = true;
    }

    private void SelectStatuePlacement()
    {
        if(isSelectingStatueLocation == true)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = ExtensionMethods.getFlooredWorldPosition(mousePosition);

            Vector3Int currentMousePositionInGrid = generalTilemap.WorldToCell(mousePosition);

            if (currentMousePositionInGrid != previous)
            {
                generalTilemap.SetTile(currentMousePositionInGrid, highlightTile);

                generalTilemap.SetTile(previous, null);

                previous = currentMousePositionInGrid;
            }

            if(Input.GetMouseButtonDown(0)){ isPlacingStatue = true; isSelectingStatueLocation = false;}
        }
        
    }

    private void PlaceStatue(LayerMask layer)
    {
        if(isPlacingStatue == true && LayerManager.PlayerIsInRealWorld())
        {
            Debug.Log("Is placing statue on Layer 1");
                
            worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            currentMousePositionInGrid = generalTilemap.WorldToCell(worldMousePosition);

            Vector3 offSetGridPosition;
            
            offSetGridPosition = new Vector3(currentMousePositionInGrid.x + 0.5f, currentMousePositionInGrid.y + 1, 0f);

            if(Input.GetMouseButtonDown(0))
            {
                Instantiate(world1Statue, offSetGridPosition, Quaternion.identity);
                isPlacingStatue = false;
            }
        }

        if(isPlacingStatue == true && !LayerManager.PlayerIsInRealWorld())
        {            
            Debug.Log("Is placing statue on Layer 2");
                
            worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            currentMousePositionInGrid = generalTilemap.WorldToCell(worldMousePosition);

            Vector3 offSetGridPosition;

            offSetGridPosition = new Vector3(currentMousePositionInGrid.x + 0.5f, currentMousePositionInGrid.y + 1, 0f);

            if(Input.GetMouseButtonDown(0))
            {
                Instantiate(world2Statue, offSetGridPosition, Quaternion.identity);
                isPlacingStatue = false;
            }
        }
    }
}
