using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class StatueManager : SerializedMonoBehaviour
{
    [FoldoutGroup("Tilemap")]
    Tilemap movementTilemap;
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

    public static bool isPlacingStatue = false;
    bool isSelectingStatueLocation = false;
    public static bool isPunchingStatue = false;
    public static bool isInRange = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
        
        movementTilemap = GameObject.FindGameObjectWithTag("Movement Tilemap").GetComponent<Tilemap>();
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

            Vector3Int currentMousePositionInGrid = movementTilemap.WorldToCell(mousePosition);

            if (currentMousePositionInGrid != previous)
            {
                movementTilemap.SetTile(currentMousePositionInGrid, highlightTile);

                movementTilemap.SetTile(previous, null);

                previous = currentMousePositionInGrid;
            }

            if(Input.GetMouseButtonDown(0)){isPlacingStatue = true; isSelectingStatueLocation = false;}
        }
    }

    private void PlaceStatue(LayerMask layer)
    {
        if(isPlacingStatue == true && LayerManager.PlayerIsInRealWorld())
        {
            worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            currentMousePositionInGrid = movementTilemap.WorldToCell(worldMousePosition);

            Vector3 offSetGridPosition;

            offSetGridPosition = new Vector3(currentMousePositionInGrid.x + 0.5f, currentMousePositionInGrid.y + 1, 0f);

            if(Input.GetMouseButtonDown(0))
            {
                GameObject previousStatue = GameObject.FindGameObjectWithTag("Statue");
                Destroy(previousStatue);

                Instantiate(world1Statue[0], offSetGridPosition, Quaternion.identity);
                isPlacingStatue = false;

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition = (mousePosition - transform.position).normalized;
                Debug.Log(mousePosition);

                anim.SetFloat("xDirection", mousePosition.x);
                anim.SetFloat("yDirection", mousePosition.y);
                anim.SetTrigger("isInvoking");
            }
        }

        if(isPlacingStatue == true && !LayerManager.PlayerIsInRealWorld())
        {
            worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            currentMousePositionInGrid = movementTilemap.WorldToCell(worldMousePosition);

            Vector3 offSetGridPosition;

            offSetGridPosition = new Vector3(currentMousePositionInGrid.x + 0.5f, currentMousePositionInGrid.y + 1, 0f);

            if(Input.GetMouseButtonDown(0))
            {
                GameObject previousStatue = GameObject.FindGameObjectWithTag("Statue");
                Destroy(previousStatue);

                Instantiate(world2Statue[0], offSetGridPosition, Quaternion.identity);
                isPlacingStatue = false;

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition = (mousePosition - transform.position).normalized;
                Debug.Log(mousePosition);

                anim.SetFloat("xDirection", mousePosition.x);
                anim.SetFloat("yDirection", mousePosition.y);
                anim.SetTrigger("isInvoking");
            }
        }
    }
}
