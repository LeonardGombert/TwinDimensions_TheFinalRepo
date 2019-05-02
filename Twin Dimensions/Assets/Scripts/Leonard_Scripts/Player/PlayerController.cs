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
    Rigidbody2D rb;

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
    #endregion
    #endregion

    #region Monobehavior Callbacks
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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

        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);

        if (horizontal != 0) vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            playerHasMoved = true;
            microMovementCooldown(movementCooldown);
            MovementCalculations(horizontal, vertical);
        }
    }

    private void MovementCalculations(int xDirection, int yDirection)
    {
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
    #endregion
    #endregion
}