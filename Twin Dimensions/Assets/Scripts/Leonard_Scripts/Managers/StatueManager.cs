using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class StatueManager : SerializedMonoBehaviour
{
    public static StatueManager instance;

    public Tilemap selectionTilemap;

    Vector3 worldMousePosition;

    Vector3Int currentMousePositionInGrid;
    
    public GameObject player;
    public GameObject world1Statue;
    public GameObject world2Statue;

    public static float statueKickSpeed = 800;

    public bool isPlacingStatue = false;
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
        //Check current player layer
        if(LayerManager.PlayerIsInRealWorld()) PlaceStatue(LayerMask.NameToLayer("Player Layer 1"));

        if(!LayerManager.PlayerIsInRealWorld()) PlaceStatue(LayerMask.NameToLayer("Player Layer 2"));

        //Check if player is placing or kicking statues
        if(PlayerInputManager.instance.GetKeyDown("placeStatue")) isPlacingStatue = true;
       
        if(PlayerInputManager.instance.GetKeyDown("kickStatue")) isKickingStatue = true;
    }

    private void PlaceStatue(LayerMask layer)
    {
        if(isPlacingStatue == true && LayerManager.PlayerIsInRealWorld())
        {
            Debug.Log("Is placing statue on Layer 1");
                
            worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePosition = ExtensionMethods.floorWithHalfOffset(worldMousePosition);

            currentMousePositionInGrid = selectionTilemap.WorldToCell(worldMousePosition);

            if(Input.GetMouseButtonDown(0))
            {
                Destroy(world1Statue, 0f);
                Instantiate(world1Statue, worldMousePosition, Quaternion.identity);
                isPlacingStatue = false;
            }
        }

        if(isPlacingStatue == true && !LayerManager.PlayerIsInRealWorld())
        {            
            Debug.Log("Is placing statue on Layer 2");
                
            worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePosition = ExtensionMethods.floorWithHalfOffset(worldMousePosition);

            currentMousePositionInGrid = selectionTilemap.WorldToCell(worldMousePosition);

            if(Input.GetMouseButtonDown(0))
            {
                Destroy(world2Statue, 0f);
                Instantiate(world2Statue, worldMousePosition, Quaternion.identity);
                isPlacingStatue = false;
            }
        }
    }
}
