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
    Vector3 originTile;
    Vector3 destinationTile;
    Vector3 newPosition;
    Vector3 lastPosition;

    Vector3 currentPosition;
    Vector3 desiredPosition;

    GameObject touchedObject;
    Animator anim;
    Rigidbody2D rb2D;
    LayerMask selectedLayerMask;
    BoxCollider2D boxCol2D;

    [FoldoutGroup("LayerMask Profiles")][SerializeField]
    LayerMask world1Profile;
    [FoldoutGroup("LayerMask Profiles")][SerializeField]
    LayerMask world2Profile;

    [FoldoutGroup("Tilemap")][SerializeField]
    Tilemap movementTilemap;

    [FoldoutGroup("Player Movement")][SerializeField]
    float movementCooldown = 0.3f;
    [FoldoutGroup("Player Movement")][SerializeField]
    float moveTime = 0.3f;
    [FoldoutGroup("Player Movement")][SerializeField]
    public static float playerMovementSpeed;

    bool playerHasMoved = false;
    bool movementIsCoolingDown = false;
    public static bool isBeingCharged = false;
    #endregion
    #endregion

    #region Monobehavior Callbacks
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();

        movementTilemap = GameObject.FindGameObjectWithTag("Movement Tilemap").GetComponent<Tilemap>();

        Physics2D.queriesStartInColliders = false;
        Physics2D.queriesHitTriggers = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(LayerManager.PlayerIsInRealWorld()) selectedLayerMask = world1Profile;
        if(!LayerManager.PlayerIsInRealWorld()) selectedLayerMask = world2Profile;
        MonitorPlayerInpus();        
    }
    #endregion

    #region Player Functions

    #region BASIC MOVEMENT ON A GRID
    private void MonitorPlayerInpus()
    {
        //Movement Overrides all other functions
        if (playerHasMoved || movementIsCoolingDown) return;

        int horizontal = 0;
        int vertical = 0;

        if(PlayerInputManager.instance.GetKey("up")) vertical = 1;
        if(PlayerInputManager.instance.GetKey("down")) vertical = -1;
        if(PlayerInputManager.instance.GetKey("left")) horizontal = -1;
        if(PlayerInputManager.instance.GetKey("right")) horizontal = 1;
              
        if (horizontal != 0) vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            PlayerAnimationsManager.isMoving = true;
            
            Vector2 destinationPosition1 = new Vector2(transform.position.x + horizontal, transform.position.y + vertical);
            Vector2 destinationPosition2 = new Vector2(horizontal, vertical);

            RaycastHit2D hit = Physics2D.Raycast(boxCol2D.bounds.center, destinationPosition2, 1, selectedLayerMask);
            Debug.DrawRay(boxCol2D.bounds.center, destinationPosition2, Color.green, 800);

            if(hit.collider)
            {
                Debug.Log("I've hit " + hit.collider.name);

                if(hit.collider.tag == "Obstacle") return;
            }

            if(!hit.collider)
            {
                playerHasMoved = true;
                microMovementCooldown(movementCooldown);
                MovementCalculations(horizontal, vertical);                
            }                        
        }

        if(horizontal == 0 && vertical == 0)
        {
            PlayerAnimationsManager.isMoving = false;
            anim.SetFloat("xDirection", horizontal);
            anim.SetFloat("yDirection", vertical);
        }
    }

    private void MovementCalculations(int xDirection, int yDirection)
    {
        anim.SetFloat("xDirection", xDirection);
        anim.SetFloat("yDirection", yDirection);

        currentPosition = movementTilemap.WorldToCell(transform.position);

        desiredPosition = movementTilemap.WorldToCell(new Vector3(currentPosition.x + xDirection, currentPosition.y + yDirection, 0));
        desiredPosition = new Vector3(desiredPosition.x + 0.5f, desiredPosition.y, 0);
        
        StartCoroutine(MoveTowards(currentPosition, desiredPosition));
    }

    private IEnumerator MoveTowards(Vector3 startPosition, Vector3 destinationPosition)
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

    void GuardStance()
    {
        if(isBeingCharged == true)
        {
            anim.SetFloat("animTypeX", 0);
            anim.SetFloat("animTypeY", 1);

            //Vector3 elephantDirection = (elephant.transform.position - transform.position).normalized;

            //anim.SetFloat("xDirection", elephantDirection.x);
            //anim.SetFloat("yDirection", elephantDirection.y);
        }
    }
    #endregion
    #endregion
}