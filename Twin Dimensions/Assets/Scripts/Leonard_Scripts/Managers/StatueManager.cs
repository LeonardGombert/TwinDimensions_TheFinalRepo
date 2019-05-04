using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class StatueManager : SerializedMonoBehaviour
{
    public static StatueManager instance;

    [FoldoutGroup("Tilemap")][SerializeField]
    Tilemap generalTilemap;
    [FoldoutGroup("Tilemap")][SerializeField]
    Tile highlightTile;

    Vector3 worldMousePosition;

    Vector3Int currentMousePositionInGrid;
    Vector3Int previous;

    GameObject player;
    
    [FoldoutGroup("Statue world 1")][SerializeField]
    List<GameObject> world1Statue = new List<GameObject>();
    [FoldoutGroup("Statue world 2")][SerializeField]
    List<GameObject> world2Statue = new List<GameObject>();

    public static float statueKickSpeed = 800;
    Animator anim;

    bool isPlacingStatue = false;
    bool isSelectingStatueLocation = false;
    public static bool isPunchingStatue = false;
    public static bool isInRange = false;

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
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
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
       
        if(PlayerInputManager.instance.GetKeyDown("kickStatue")) isPunchingStatue = true;
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
                Instantiate(world1Statue[0], offSetGridPosition, Quaternion.identity);
                isPlacingStatue = false;
                Vector3 statueDirection;
                statueDirection = (world1Statue[0].transform.position - transform.position).normalized;
                
                anim.SetFloat("xDirection", statueDirection.x);
                anim.SetFloat("yDirection", statueDirection.y);

                anim.SetFloat("animTypeX", 0);
                anim.SetFloat("animTypeY", -1);
            }
        }

        if(isPlacingStatue == true && !LayerManager.PlayerIsInRealWorld())
        {
            worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            currentMousePositionInGrid = generalTilemap.WorldToCell(worldMousePosition);

            Vector3 offSetGridPosition;

            offSetGridPosition = new Vector3(currentMousePositionInGrid.x + 0.5f, currentMousePositionInGrid.y + 1, 0f);

            if(Input.GetMouseButtonDown(0))
            {
                Instantiate(world2Statue[0], offSetGridPosition, Quaternion.identity);
                isPlacingStatue = false;

                Vector3 statueDirection;
                statueDirection = (world1Statue[0].transform.position - transform.position).normalized;

                anim.SetFloat("xDirection", statueDirection.x);
                anim.SetFloat("yDirection", statueDirection.y);

                anim.SetFloat("animTypeX", 0);
                anim.SetFloat("animTypeY", -1);
            }
        }
    }
}
