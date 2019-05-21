using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Cinemachine;
using Cinemachine.Editor;

public class ElephantController : MonsterClass
{
    #region Variable Declarations
    #region //RAYCAST DETECTION
    [FoldoutGroup("Raycast Detection Bools")][SerializeField]
    bool lookingForPlayer = true;
    [FoldoutGroup("Raycast Detection Bools")][SerializeField]
    bool lookingForWall = false;
    [FoldoutGroup("Raycast Detection Bools")][SerializeField]
    public bool isRushingPlayer = false;

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

    #region //POSITIONS
    Vector3 playerDirection;
    Vector3 wallPosition;
    Vector3 wallPointCoordinates;

    Vector3 currentPositionOnGrid;
    Vector3 centeredPositionOnGrid;

    Vector3Int currentSelectedDirection;
    Vector3Int previousSelectedDirection;
    List<Vector3> CardinalDirections = new List<Vector3>();
    #endregion

    #region //TILEMAP
    [FoldoutGroup("Tilemap")][SerializeField] 
    Tile highlightTile;
    [FoldoutGroup("Tilemap")][SerializeField]
    Tilemap movementTilemap;
    #endregion

    #region //CAMERAS
    [FoldoutGroup("Cinemachine Virtual Cameras")][SerializeField]
    CinemachineVirtualCamera elephantCamera = new CinemachineVirtualCamera();
    [FoldoutGroup("Cinemachine Virtual Cameras")][SerializeField]
    CinemachineVirtualCamera playerCamera = new CinemachineVirtualCamera();
    #endregion

    #region //LAYERS
    [FoldoutGroup("LayerMask Profiles")][SerializeField]
    LayerMask world1Profile;
    [FoldoutGroup("LayerMask Profiles")][SerializeField]
    LayerMask world2Profile;
    #endregion

    #region //GENERAL VARIABLES
    Transform target;
    Rigidbody2D rb2D;
    BoxCollider2D boxCol2D;
    #endregion     

    #region //TURRET ACTIVATION
    bool isActive;
    bool isTriggered = false;
    int currentIndexNumber;
    int maxIndexNumber;
    #endregion

    #region //SOUND EFFECTS
    [FoldoutGroup("Player SFX")][SerializeField] AudioClip[] startCharging;
    [FoldoutGroup("Player SFX")][SerializeField] AudioClip[] chargeSounds;
    [FoldoutGroup("Player SFX")][SerializeField] AudioClip[] collisionSounds;
    [FoldoutGroup("Player SFX")][SerializeField] AudioClip[] deathSounds;
    #endregion
    #endregion

    #region Monobehavior Callbacks
    public override void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();

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
        
        TriggerBehavior();

        MonitorSFX();
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
                //Debug.DrawRay(transform.position, direction, Color.green, 80f);
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
                        if(rangeDetection.collider.tag == "Player")
                        {
                            lookingForWall = true;
                            isRushingPlayer = true;
                            LookForWall(direction);
                            rangeDetection.collider.gameObject.SendMessage("PlayerDied", playerDirection);
                        }
                        
                        else if (rangeDetection.collider.tag == "Statue")
                        {
                            lookingForWall = true;
                            LookForWall(direction);
                            break;
                        }
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
            //Debug.DrawRay(transform.position, direction);

            if (wallSeeker.collider.tag == "Obstacle")
            {
                wallPointCoordinates = new Vector3(wallSeeker.point.x, wallSeeker.point.y);
                
                //currentPositionOnGrid = movementTilemap.WorldToCell(wallPointCoordinates);

                centeredPositionOnGrid = new Vector3(wallPointCoordinates.x, wallPointCoordinates.y, 0);

                if(direction == Right) centeredPositionOnGrid = new Vector3(centeredPositionOnGrid.x -.5f, centeredPositionOnGrid.y);
                if(direction == Left) centeredPositionOnGrid = new Vector3(centeredPositionOnGrid.x +.5f, centeredPositionOnGrid.y);
                if(direction == Up) centeredPositionOnGrid = new Vector3(centeredPositionOnGrid.x, centeredPositionOnGrid.y -.5f);
                if(direction == Down) centeredPositionOnGrid = new Vector3(centeredPositionOnGrid.x, centeredPositionOnGrid.y +.5f);

                isCharging = true;
                lookingForWall = false;
                StartCoroutine(Charging(centeredPositionOnGrid, direction));
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
            if (gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1")) mask = world1Profile;
            else if (gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2")) mask = world2Profile;
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
            maxDirection = .5f;
            if (gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1")) mask = LayerMask.GetMask("World Obstacle Detection 1");
            else if (gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2")) mask = LayerMask.GetMask("World Obstacle Detection 2");
        }

        return Physics2D.Raycast(transform.position, direction, maxDirection, mask);
    }
    #endregion

    #region  //CHARGE
    private IEnumerator Charging(Vector3 destination, Vector3 direction)
    {
        Cinemachine.NoiseSettings.NoiseParams noiseCam;
        noiseCam.Amplitude = 0.5f;
        noiseCam.Frequency = 9;

        playerDirection = (target.position - transform.position).normalized;

        anim.SetFloat("MoveX", playerDirection.x);
        anim.SetFloat("MoveY", playerDirection.y);

        float sqrRemainingDistanceToDestination = (transform.position - destination).sqrMagnitude;
        float inverseMoveTime = 1 / chargeSpeed;
        
        Vector2 destinationPosition = new Vector2(direction.x, direction.y);

        while (sqrRemainingDistanceToDestination > float.Epsilon)
        {      
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
    
    public override void ActivateTriggerBehavior()
    {
        Debug.Log("I'm triggered");
        isTriggered = true;           
    }
    
    void TriggerBehavior()
    {
        if(isTriggered)
        { 
            playerCamera.gameObject.SetActive(false);
            elephantCamera.gameObject.SetActive(true);
            
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
                elephantCamera.gameObject.SetActive(false);
                playerCamera.gameObject.SetActive(true);
            }
        }
        else return;
    }
    #endregion

    public override void MonitorSFX()
    {
        if(lookingForWall == true) SoundManager.instance.RandomizeSfx(startCharging);
        if(isCharging == true) SoundManager.instance.RandomizeSfx(chargeSounds);
        //if(hasTouchedWall == true) SoundManager.instance.RandomizeSfx(collisionSounds);
        //if(isDead== true) SoundManager.instance.RandomizeSfx(deathSounds);
        else return;
    }
}