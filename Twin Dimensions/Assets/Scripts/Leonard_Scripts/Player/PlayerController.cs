using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class PlayerController : SerializedMonoBehaviour
{
    #region Variable Decarations
    
    #region //BASIC MOVEMENT
    private Vector3 originTile;
    private Vector3 destinationTile;
    private Vector3 newPosition;
    private Vector3 lastPosition;

    private Vector3 currentPositionOnGrid;
    private Vector3 desiredPositionOnGrid;

    private GameObject touchedObject;
    public Tilemap movementTilemap;

    private Animator anim;
    Rigidbody2D rb;

    [FoldoutGroup("Player Movement")][SerializeField]
    private float movementCooldown = 0.3f;
    [FoldoutGroup("Player Movement")][SerializeField]
    private float moveTime = 0.3f;
    [FoldoutGroup("Player Movement")][SerializeField]
    public static float playerMovementSpeed;

    [SerializeField]
    private bool playerHasMoved = false;
    private bool movementIsCoolingDown = false;
    private bool blinkIsCoolingdown = false;
    #endregion

    #region //ON HIT COLOR CHANGE

    public SpriteRenderer sr;
    private Color[] colors = { Color.yellow, Color.red };

    private bool hitEnemy = false;
    private bool hitTrap = false;
    private bool hitStaticEnvironment = false;

    //TeleportationManager tManager;
    
    GameObject firstPortal;
    
    GameObject manager;
    #endregion

    #endregion

    #region Monobehavior Callbacks
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        manager = GameObject.FindGameObjectWithTag("Manager");
    }

    // Update is called once per frame
    void Update()
    {
        ConvertPlayerMovementInputs();

        PlayerHit();       
    }
    #endregion

    #region Player Functions

    #region BASIC MOVEMENT ON A GRID
    private void ConvertPlayerMovementInputs()
    {
        //Movement Overrides all other functions
        if (playerHasMoved || movementIsCoolingDown) return;

        int horizontal = 0;
        int vertical = 0;

        if(PlayerInputManager.instance.GetKey("up")) vertical = 1;
        if(PlayerInputManager.instance.GetKey("down")) vertical = -1;
        if(PlayerInputManager.instance.GetKey("left")) horizontal = -1;
        if(PlayerInputManager.instance.GetKey("right")) horizontal = 1;

        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);

        if (horizontal != 0) vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            playerHasMoved = true;
            microMovementCooldown(movementCooldown);
            CalculateMovement(horizontal, vertical);
        }
    }

    private void CalculateMovement(int xDirection, int yDirection)
    {
        currentPositionOnGrid = movementTilemap.WorldToCell(transform.position);

        desiredPositionOnGrid = movementTilemap.WorldToCell(new Vector3(currentPositionOnGrid.x + xDirection, currentPositionOnGrid.y + yDirection, 0));

        desiredPositionOnGrid = new Vector3(desiredPositionOnGrid.x + 0.5f, desiredPositionOnGrid.y + 0.75f, 0);

        RaycastHit2D rangeDetection = Physics2D.Raycast(currentPositionOnGrid, desiredPositionOnGrid);

        Debug.DrawRay(currentPositionOnGrid, desiredPositionOnGrid, Color.green);

         StartCoroutine(MoveToCell(currentPositionOnGrid, desiredPositionOnGrid));
/*
        if(rangeDetection.collider)
        {
            Debug.Log(rangeDetection.collider.gameObject.name);

            if(CollisionManager.IsWalkable()) StartCoroutine(MoveToCell(currentPositionOnGrid, desiredPositionOnGrid));

            else if(!CollisionManager.IsWalkable()) return;

            else return;
        }

        else return;*/
    }

    private IEnumerator MoveToCell(Vector3 startPosition, Vector3 destinationPosition)
    {
        float sqrRemainingDistanceToDestination = (transform.position - destinationPosition).sqrMagnitude;
        float inverseMoveTime = 1 / moveTime;

        while (sqrRemainingDistanceToDestination > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationPosition, inverseMoveTime * Time.unscaledDeltaTime);
            sqrRemainingDistanceToDestination = (transform.position - destinationPosition).sqrMagnitude;

            yield return null;
        }

        playerHasMoved = false;
    }

    private IEnumerator microMovementCooldown(float cooldown)
    {
        movementIsCoolingDown = true;
        while (cooldown > 0f)
        {
            cooldown -= Time.unscaledDeltaTime;
            yield return null;
        }

        movementIsCoolingDown = false;
    }
    #endregion

    #region OTHER
    private void PlayerHit()
    {
        if (hitEnemy == true)
        {
            //Debug.Log("I have hit an enemy");
            sr.color = new Color(2, 0, 0, 255);
            hitEnemy = false;
        }

        if (hitTrap == true)
        {
            //Debug.Log("I have hit a trap");
            sr.color = new Color(2, 0, 0, 255);
            hitTrap = false;
        }

        if (hitStaticEnvironment == true)
        {
            //Debug.Log("I have hit the environment");
            sr.color = new Color(2, 0, 0, 255);
            hitStaticEnvironment = false;
        }

        sr.color = Color.Lerp(sr.color, Color.white, Time.unscaledDeltaTime / 0.5f); //FLASH - lerps from white to red over 1 second
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Sand Shard")
        {
            SandManager.AddNewSandShard(1);
        }
    }
    #endregion
}