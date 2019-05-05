using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class ElephantController : MonsterClass
{
    #region Variable Declarations
    #region //RAYCAST DETECTION
    [FoldoutGroup("Raycast Detection Bools")][SerializeField]
    bool lookingForPlayer = true;
    [FoldoutGroup("Raycast Detection Bools")][SerializeField]
    bool lookingForWall = false;

    bool secondaryWallDetection = false;
    bool isAnimatorFacingDirection = false;
    
    Vector3 Up = new Vector3(0, 1), Right = new Vector3(1, 0), Down = new Vector3(0, -1), Left = new Vector3(-1, 0);
    #endregion

    #region //CHARGING
    [FoldoutGroup("Charge Variables")][SerializeField]
    private bool isCharging = false;
    [FoldoutGroup("Charge Variables")][SerializeField]
    float chargeSpeed = 0.25f;
    [FoldoutGroup("Charge Variables")][SerializeField]
    float chargeRadiusInTiles;
    #endregion

    Vector3 playerDirection;
    Vector3 wallPosition;
    Vector3 wallPointCoordinates;

    Vector3 currentPositionOnGrid;
    Vector3 centeredPositionOnGrid;

    Vector3Int currentSelectedDirection;
    Vector3Int previousSelectedDirection;

    public Tile highlightTile;

    [FoldoutGroup("LayerMask Profiles")][SerializeField]
    LayerMask world1Profile;
    [FoldoutGroup("LayerMask Profiles")][SerializeField]
    LayerMask world2Profile;

    [FoldoutGroup("Tilemap")][SerializeField]
    Tilemap movementTilemap;

    Transform target;

    Rigidbody2D rb2D;
    Camera myCenteredCam;

    bool isActive;
    bool isTriggered = false;

    int currentIndexNumber = 0;
    int maxIndexNmber = 0;

    List<Vector3> CardinalDirections = new List<Vector3>();
    #endregion

    #region Monobehavior Callbacks
    public override void  Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();        
        myCenteredCam = GetComponentInChildren<Camera>();

        movementTilemap = GameObject.FindGameObjectWithTag("Movement Tilemap").GetComponent<Tilemap>();

        target = GameObject.FindWithTag("Player").transform;

        CardinalDirections.AddRange(new Vector3[] { Up, Right, Down, Left });
    }
    
    public override void Update()
    {
        base.CheckIfBeingTeleported();

        base.CheckBehaviorMode();

        CheckBehaviorModeInOtherWorld();
        
        LookForPlayer();
        
        int maxIndexNmber = CardinalDirections.Count;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f  && currentIndexNumber <= maxIndexNmber) currentIndexNumber += 1;

        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentIndexNumber > 0) currentIndexNumber -= 1;

        if(currentIndexNumber >= maxIndexNmber) currentIndexNumber = 0;

        if(isTriggered) ConfirmDirection();
    }
    #endregion

    #region Elephant Functions

    #region //RAYCAST CHECKS
    private void LookForPlayer()
    {
        if(isInActiveMode == true)
        {
            foreach (Vector3 direction in CardinalDirections)
            {
                if (lookingForPlayer == true && isCharging == false)
                {
                    RaycastHit2D rangeDetection = RaycastManager(direction, lookingForPlayer);

                    RaycastHit2D facingDirection = RaycastManager(target.position, isAnimatorFacingDirection);
                        
                    playerDirection = (target.position - transform.position);

                    if (isCharging == false)
                    {
                        anim.SetFloat("MoveX", playerDirection.x);
                        anim.SetFloat("MoveY", playerDirection.y);
                    }

                    if (rangeDetection.collider)
                    {
                        if (rangeDetection.collider.tag == "Player" || rangeDetection.collider.tag == "Statue")
                        {
                            lookingForWall = true;
                            LookForWall(direction);
                        }
                        break;
                    }
                }
            }
        }

        else return;
    }

    private void LookForWall(Vector3 direction)
    {
        if (lookingForWall == true)
        {
            RaycastHit2D wallSeeker = RaycastManager(direction, lookingForWall);
            Debug.DrawRay(transform.position, direction);

            if (wallSeeker.collider.tag == "Obstacle")
            {
                wallPointCoordinates = new Vector3(wallSeeker.point.x, wallSeeker.point.y);
                
                currentPositionOnGrid = movementTilemap.WorldToCell(wallPointCoordinates);

                centeredPositionOnGrid = new Vector3(wallPointCoordinates.x + 0, wallPointCoordinates.y + 0, 0);

                isCharging = true;
                lookingForWall = false;
                StartCoroutine(Charging(centeredPositionOnGrid));
            }
        }
    }

    private RaycastHit2D RaycastManager(Vector3 direction, bool rayCastType)
    {
        float maxDirection = 0f;

        LayerMask mask = LayerMask.GetMask("Default");

        if (rayCastType == lookingForPlayer)
        {
            maxDirection = chargeRadiusInTiles;
            if (gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1")) mask = LayerMask.GetMask("Player Layer 1");
            else if (gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2")) mask = LayerMask.GetMask("Player Layer 2");
        }

        if (rayCastType == lookingForWall)
        {
            maxDirection = 50;
            if (gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1")) mask = LayerMask.GetMask("World Obstacle Detection 1");
            else if (gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2")) mask = LayerMask.GetMask("World Obstacle Detection 2");
        }

        if (rayCastType == isAnimatorFacingDirection)
        {
            maxDirection = float.PositiveInfinity;
        }

        if (rayCastType == secondaryWallDetection)
        {
            maxDirection = 0.5f;
            if (gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1")) mask = LayerMask.GetMask("World Obstacle Detection 1");
            else if (gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2")) mask = LayerMask.GetMask("World Obstacle Detection 2");
        }

        return Physics2D.Raycast(transform.position, direction, maxDirection, mask);
    }
    #endregion

    #region  //CHARGE
    private IEnumerator Charging(Vector3 destination)
    {
        playerDirection = (target.position - transform.position).normalized;

        anim.SetFloat("MoveX", playerDirection.x);
        anim.SetFloat("MoveY", playerDirection.y);

        float sqrRemainingDistanceToDestination = (transform.position - destination).sqrMagnitude;
        float inverseMoveTime = 1 / chargeSpeed;

        while (sqrRemainingDistanceToDestination > float.Epsilon)
        {
            /*RaycastHit2D chargeWallDetection = Physics2D.Raycast(transform.position, new Vector2(0,1f));
            Debug.DrawRay(transform.position, new Vector2(0, .5f), Color.green, 5f);
            
            if (chargeWallDetection.collider.tag == "Obstacle")
            {
                Debug.Log("I've detected a wall");
                if (chargeWallDetection.collider.tag == "Obstacle") sqrRemainingDistanceToDestination = transform.position.sqrMagnitude;
            }*/

            transform.position = Vector3.MoveTowards(transform.position, destination, chargeSpeed * Time.deltaTime);
            sqrRemainingDistanceToDestination = (transform.position - destination).sqrMagnitude;

            yield return null;
        }

        isCharging = false;
        lookingForPlayer = true;
    }
    #endregion

    #region //WORLD SWITCHING
    private void CheckBehaviorModeInOtherWorld()
    {
        if (isInAltMode == true)
        {
            sr.sprite = spriteList[0];
            anim.enabled = false;
        }

        else if(isInActiveMode == true)
        {
            sr.sprite = spriteList[1];
            anim.enabled = true;
        }           
    }
    #endregion    
    
    public override void TriggerBehavior()
    {
        Debug.Log("I'm triggered");
        isTriggered = true;           
    }
    
    void ConfirmDirection()
    {
        if(isTriggered)
        {            
            anim.SetFloat("MoveX", CardinalDirections[currentIndexNumber].x);
            anim.SetFloat("MoveY", CardinalDirections[currentIndexNumber].y);

            //highlight 4 squares around, representing directions
            
            currentSelectedDirection = movementTilemap.WorldToCell(CardinalDirections[currentIndexNumber]);

            if (currentSelectedDirection != previousSelectedDirection)
            {
                movementTilemap.SetTile(currentSelectedDirection, highlightTile);

                movementTilemap.SetTile(previousSelectedDirection, null);

                previousSelectedDirection = currentSelectedDirection;
            } 

            if(PlayerInputManager.instance.GetKeyDown("chargeElephant"))
            {
                lookingForWall = true;
                isCharging = true;
                LookForWall(currentSelectedDirection);
                isTriggered = false;    
            }
        }
        else return;
    }
    #endregion
}