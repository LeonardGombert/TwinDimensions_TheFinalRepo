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
    
    [SerializeField]
    List<GameObject> playerStatue = new List<GameObject>();

    public GameObject player;

    public static float statueKickSpeed = 800;

    public bool isPlacingStatue = false;
    public static bool isKickingStatue = false;
    public static bool isInRange = false;

    public GameObject[] statues;

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
        CheckPlayerLayer();

        if(PlayerInputManager.instance.KeyDown("placeStatue"))
        {
            isPlacingStatue = true;
        }
       
        if(PlayerInputManager.instance.KeyDown("kickStatue"))
        {
            isKickingStatue = true;
        }        
    }

    private void CheckPlayerLayer()
    {
        if(LayerManager.PlayerIsInRealWorld())
        {
            CheckIfPlacingStatue(LayerMask.NameToLayer("Player Layer 1"));
        }

        if(!LayerManager.PlayerIsInRealWorld())
        {
            CheckIfPlacingStatue(LayerMask.NameToLayer("Player Layer 2"));
        }
    }

    private void CheckIfPlacingStatue(LayerMask layer)
    {
        if(isPlacingStatue == true && LayerManager.PlayerIsInRealWorld())
        {
            Debug.Log("Is placing statue on Layer 1");
                
            worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePosition = ExtensionMethods.floorWithHalfOffset(worldMousePosition);

            currentMousePositionInGrid = selectionTilemap.WorldToCell(worldMousePosition);

            if(Input.GetMouseButtonDown(0))
            {
                statues = GameObject.FindGameObjectsWithTag("Statue");

                foreach(GameObject statue in statues) Destroy(statue);

                Instantiate(playerStatue[0], worldMousePosition, Quaternion.identity);
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
                statues = GameObject.FindGameObjectsWithTag("Statue");

                foreach(GameObject statue in statues) Destroy(statue);

                Instantiate(playerStatue[1], worldMousePosition, Quaternion.identity);
                isPlacingStatue = false;
            }
        }
    }
}
