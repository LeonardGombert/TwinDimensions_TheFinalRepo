using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class KaliBossAI : MonoBehaviour
{ 
    #region Variable Declarations
    #region //GENERAL VARIABLES
    [FoldoutGroup("General")][SerializeField] Animator anim;
    [FoldoutGroup("General")][SerializeField] GameObject player;
    //public StateMachine<KaliBossAI> stateMachine { get; set; }
    #endregion

    #region //BOSS STATES AND STAGES
    S1BossStates defautState = S1BossStates.S1Idle;
    [FoldoutGroup("KaliStats")][SerializeField] BossStages bossStage;
    [FoldoutGroup("KaliStats")][SerializeField] S1BossStates Stage1CurrentState;
    [FoldoutGroup("KaliStats")][SerializeField] S2BossStates Stage2CurrentState;
    [FoldoutGroup("KaliDebug")][SerializeField] bool attackState = false;
    [FoldoutGroup("KaliDebug")][SerializeField] bool idleState = false;
    [FoldoutGroup("KaliDebug")][SerializeField] bool deathState = false;
    #endregion
    
    #region //KALI STATS
    [FoldoutGroup("KaliStats")][SerializeField] float lifePoints;
    [FoldoutGroup("KaliStats")][SerializeField] float lifepointsToChangeState;
    [FoldoutGroup("KaliStats")][SerializeField] float damageValue;
    [FoldoutGroup("KaliDebug")][SerializeField] bool damageDealt = false;
    [FoldoutGroup("CollisionDebug")][SerializeField] GameObject leftMapDetectionCollider;
    [FoldoutGroup("CollisionDebug")][SerializeField] GameObject rightMapDetectionCollider;
    [FoldoutGroup("CollisionDebug")][SerializeField] GameObject currentPlayerActiveSideCollider;
    #endregion
    
    #region //SLAM ATTACK VARIABLES
    [FoldoutGroup("AttackDebug")][ShowInInspector]  public static bool isSlamming = false;
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool isTrackingForSlam = true;
    [FoldoutGroup("SlamAttack")][SerializeField] float timeHoldingSlam = 0;
    [FoldoutGroup("SlamAttack")][SerializeField] float timeToHoldSlam = 2f;

    #endregion

    #region //LASER BEAM VARIABLES
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool usingLaser = false;
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool isTrackingForLaser = true;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] LineRenderer laserBeam;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserLeftPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserRightPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserSpawnPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserHitPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserStartPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserEndPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] float laserMoveTime;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] float timeHoldingLaser = 0f;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] float timeToHoldLaser = 1.75f;
    #endregion

    #region //SWEEP ATTACK VARIABLES
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool isSweeping = false;
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool trackPlayerForSweep = true;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform kaliBasePosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepLeftPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepRightPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepStartPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepEndPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepCurrentPosition;
    #endregion

    #region //STATE CHANGE VARIABLES
    [FoldoutGroup("ChangeStateDebug")][SerializeField] float maxTimeBetweenStates;
    [FoldoutGroup("ChangeStateDebug")][SerializeField] float minAttackValue;
    [FoldoutGroup("ChangeStateDebug")][SerializeField] float timeTillStateChange;
    [FoldoutGroup("ChangeStateDebug")][SerializeField] float attackProbabilityBooster;
    
    [FoldoutGroup("ChangeStateDebug")][SerializeField] float minWaitTime;
    [FoldoutGroup("ChangeStateDebug")][SerializeField] float maxWaitTime;
    [FoldoutGroup("ChangeStateDebug")][SerializeField] float currentRunTime;
    [FoldoutGroup("ChangeStateDebug")][SerializeField] float timeTillNextAttack;
    [FoldoutGroup("ChangeStateDebug")][SerializeField] float probabilityBooster;
    #endregion

    #region //KALI ENUM STATES
    public enum BossStages {Stage1,Stage2,}

    public enum S1BossStates {S1Idle,S1Attacking,S1Dead,}

    public enum S2BossStates {S2Idle,S2Attacking,S2Dead,}
    #endregion
    #endregion

    #region Monobehavior Callbacks
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        sweepCurrentPosition = this.gameObject.transform;
        //playerSideDetectionColliders.AddRange(new GameObject[] {rightSlamAttackBoxCol2D, leftSlamAttackBoxCol2D});
        //stateMachine = new StateMachine<KaliBossAI>(this);
        //stateMachine.ChangeState(IdleState.Instance);
    }

    // Update is called once per frame
    void Update()
    {
        //Debugging, used to test attacking Kali
        if(Input.GetKeyDown(KeyCode.F))
        {
            lifePoints -= damageValue;
            //StartCoroutine(StateSwitch());
        }

        //stateMachine.Update();
        StateSwitching();

        WatchForStageChange();
        
        if(bossStage == BossStages.Stage1) Stage1StateManager();
        if(bossStage == BossStages.Stage2) Stage2StateManager();

        UpdateCurrentState();
    }
    #endregion

    #region //CHECK PLAYER SIDE
    private void UpdatePlayerSide(GameObject side)
    {
        currentPlayerActiveSideCollider = side.gameObject;

        if(currentPlayerActiveSideCollider == rightMapDetectionCollider)
        {
            laserStartPosition = laserLeftPosition; //if player is on the right, sweep from left
            laserEndPosition = laserRightPosition;
        }

        else if(currentPlayerActiveSideCollider == leftMapDetectionCollider)
        {
            laserStartPosition = laserRightPosition; //if player is on the left, sweep from right
            laserEndPosition = laserLeftPosition;
        }

        if(currentPlayerActiveSideCollider == rightMapDetectionCollider)
        {
            sweepStartPosition = sweepRightPosition; //if player is on the right, sweep from left
            sweepEndPosition = sweepLeftPosition;
        }

        else if(currentPlayerActiveSideCollider == leftMapDetectionCollider)
        {
            sweepStartPosition = sweepLeftPosition; //if player is on the left, sweep from right
            sweepEndPosition = sweepRightPosition;
        }
    }
    #endregion
    
    #region //ATTACK MANAGER
    void WatchForStageChange()
    {
        if(lifePoints <= lifepointsToChangeState) bossStage = BossStages.Stage2;
        else return;
    }

    void Stage1StateManager()
    {
        timeTillNextAttack = Random.Range (minWaitTime, maxWaitTime);

        if (Stage1CurrentState == S1BossStates.S1Attacking)
        {
            if (currentRunTime >= timeTillNextAttack)
            {
                StartCoroutine(SlamAttack());
                currentRunTime = 0;
            }
            else currentRunTime += Time.deltaTime;
        }

        //if(Stage1CurrentState == S1BossStates.S1Idle) StartCoroutine(IdleState());
        //if(Stage1CurrentState == S1BossStates.S1Dead) StartCoroutine(DeathState()); 
    }

    void Stage2StateManager()
    {
        timeTillNextAttack = Random.Range (minWaitTime, maxWaitTime);

        if (Stage2CurrentState == S2BossStates.S2Attacking)
        {
            if (currentRunTime >= timeTillNextAttack)
            {
                RandomizeAttacks();
                currentRunTime = 0;
            }
            else currentRunTime += Time.deltaTime;
        }

        if(Stage2CurrentState == S2BossStates.S2Idle) StartCoroutine(IdleState());
        //if(Stage2CurrentState == S2BossStates.S2Dead) StartCoroutine(DeathState());
    }

    void RandomizeAttacks()
    {
        int whichAttack  = Random.Range(0, 3);

        Debug.Log("I'm attacking with " + whichAttack);

        if(whichAttack == 0) StartCoroutine(SlamAttack());
        if(whichAttack == 1) StartCoroutine(MoveToSweepAttackLocation());
        if(whichAttack == 2) StartCoroutine(LaserEyeBeam());

        Stage2CurrentState = S2BossStates.S2Idle;
    }
    #endregion

    #region //STATE CHANGING

    void StateSwitching()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            if(bossStage == BossStages.Stage1)Stage1CurrentState = S1BossStates.S1Attacking;
            else if(bossStage == BossStages.Stage2) StartCoroutine(LaserEyeBeam()); //Stage2CurrentState = S2BossStates.S2Attacking;
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            if(bossStage == BossStages.Stage1)Stage1CurrentState = S1BossStates.S1Idle;
            else if(bossStage == BossStages.Stage2) StartCoroutine(MoveToSweepAttackLocation()); //Stage2CurrentState = S2BossStates.S2Idle;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            if(bossStage == BossStages.Stage1)Stage1CurrentState = S1BossStates.S1Dead;
            else if(bossStage == BossStages.Stage2) StartCoroutine(SlamAttack()); //Stage2CurrentState = S2BossStates.S2Dead;
        }
    }

    IEnumerator StateSwitch(float timeTillStateChange)
    {
        timeTillStateChange = Random.Range(0, maxTimeBetweenStates);
        currentRunTime += Time.deltaTime;

        if(bossStage == BossStages.Stage1)
        {
            if(currentRunTime <= timeTillStateChange && damageDealt) currentRunTime += attackProbabilityBooster; //does not attack, raise probability nothing happens
            else if(currentRunTime >= timeTillStateChange) Stage1CurrentState = S1BossStates.S1Attacking; //attacks, repeat
        }

        else if(bossStage == BossStages.Stage2)
        {
            if(currentRunTime <= timeTillStateChange && damageDealt) currentRunTime += attackProbabilityBooster; //does not attack, raise probability nothing happens
            else if(timeTillStateChange >= currentRunTime) Stage2CurrentState = S2BossStates.S2Attacking; //attacks, repeat
        }
        
        yield return null;
    }
    
    void UpdateCurrentState()
    {
        if(bossStage == BossStages.Stage1)
        {
            switch(Stage1CurrentState)
            {
                case S1BossStates.S1Idle :
                attackState = false;
                idleState = true;
                deathState = false;
                break;
                
                case S1BossStates.S1Dead :
                attackState = false;
                idleState = false;
                deathState = true;
                break;

                case S1BossStates.S1Attacking :
                attackState = true;
                idleState = false;
                deathState = false;
                break;

                default :
                Debug.Log("null");
                break;
            } 
        }

        else if(bossStage == BossStages.Stage2)
        {
            switch(Stage2CurrentState)
            {            
                case S2BossStates.S2Idle :
                attackState = false;
                idleState = true;
                deathState = false;
                break;

                case S2BossStates.S2Dead :
                attackState = false;
                idleState = false;
                deathState = true;
                break;

                case S2BossStates.S2Attacking :
                attackState = true;
                idleState = false;
                deathState = false;
                break;

                default :
                Debug.Log("null");
                break;
            }
        }        
    }
    #endregion

    #region //ATTACK STATES
    IEnumerator SlamAttack()
    {
        isSlamming = true;
        isTrackingForSlam = false;
        timeHoldingSlam = 0;

        while(true)
        {
            anim.SetBool("S1SlamAttack", true);
            
            timeHoldingSlam += Time.deltaTime;

            if(timeHoldingSlam >= timeToHoldSlam) // If Kali has held long enough...
            {
                anim.SetBool("S1SlamAttack", false);
                currentPlayerActiveSideCollider.SendMessage("Slamming");
                Stage1CurrentState = S1BossStates.S1Idle;
                Stage2CurrentState = S2BossStates.S2Idle;
                isTrackingForSlam = true;
                isSlamming = false;
                yield break; //...stop the coroutine
            }
            
            yield return null; // Otherwise, continue next frame
        }
    }
    
    IEnumerator MoveToSweepAttackLocation()
    {
        trackPlayerForSweep = false;
        float timeSinceStarted = 0f;

        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, sweepStartPosition.position, timeSinceStarted);

            if (transform.position == sweepStartPosition.position) // If the object has arrived...
            {
                StartCoroutine(SweepAttack());
                yield break; //...stop the coroutine
            }           
            yield return null; // Otherwise, continue next frame
        }
    }

    IEnumerator SweepAttack()
    {
        isSweeping = true;

        float timeSinceStarted = 0f;

        while (true)
        {
            anim.SetBool("S2SweepAttack", true);

            timeSinceStarted += Time.deltaTime;
            transform.position = Vector3.Lerp(sweepLeftPosition.position, sweepEndPosition.position, timeSinceStarted);

            if (transform.position == sweepEndPosition.position) // If the object has arrived...
            {
                transform.position = Vector3.Lerp(transform.position, kaliBasePosition.position, timeSinceStarted);
                anim.SetBool("S2SweepAttack", false);
                currentPlayerActiveSideCollider.SendMessage("Sweeping");   
                Stage2CurrentState = S2BossStates.S2Idle;
                trackPlayerForSweep = true;
                isSweeping = false;
                
                yield break; //...stop the coroutine
            }
           
            yield return null; // Otherwise, continue next frame
        }
    }

    public IEnumerator LaserEyeBeam()
    {
        laserBeam.enabled = true;
        usingLaser = true;
        timeHoldingLaser = 0;

        float sqrRemainingDistanceToDestination = (laserHitPosition.transform.position - laserEndPosition.transform.position).sqrMagnitude;
        float timeSinceStarted = 0f;

        while (true)
        {
            timeHoldingLaser += Time.deltaTime;
            anim.SetBool("S2Laser", true);

            if (timeHoldingLaser >= timeToHoldLaser)
            {
                timeSinceStarted += Time.deltaTime;

                RaycastHit2D hit = Physics2D.Raycast(laserSpawnPosition.transform.position, laserHitPosition.transform.position);
                Debug.DrawLine(laserSpawnPosition.transform.position, laserHitPosition.transform.position, Color.green);
                if (hit.collider)
                {
                    Debug.Log("I hit " + hit.collider.name);

                    if (hit.collider.gameObject.tag == "Player")
                    {
                        Debug.Log("Have touched the player");
                        //laserHitPosition.transform.position = new Vector3(hit.point.x, hit.point.y);
                    }
                }

                laserHitPosition.transform.position = laserStartPosition.transform.position;
                laserHitPosition.transform.position = Vector3.Lerp(laserStartPosition.transform.position, laserEndPosition.transform.position, timeSinceStarted);

                laserBeam.SetPosition(0, laserSpawnPosition.transform.position); //defines 1st ("start") point
                laserBeam.SetPosition(1, laserHitPosition.transform.position); //defines 2nd (or "end") point

                if (laserHitPosition.transform.position == laserEndPosition.transform.position) // If the object has arrived...
                {
                    anim.SetBool("S2Laser", false);
                    usingLaser = false;
                    laserBeam.enabled = false;
                    Stage2CurrentState = S2BossStates.S2Idle;
                    yield break; //...stop the coroutine
                }

                yield return null; // Otherwise, continue next frame
            }

            yield return null;
        }
    }
    #endregion

    #region //IDLE_OTHER STATES

    IEnumerator IdleState()
    {
        yield return null;
    }
    #endregion
}