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
    #endregion
    
    #region //SLAM ATTACK VARIABLES
    [FoldoutGroup("AttackDebug")][ShowInInspector]  public static bool isSlamming = false;
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool isTrackingForSlam = true;
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject rightSlamAttackBoxCol2D;
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject leftSlamAttackBoxCol2D;
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject activeSlamAttackBoxCol2D;
    [FoldoutGroup("SlamAttack")][SerializeField] float timeHoldingSlam = 0;
    [FoldoutGroup("SlamAttack")][SerializeField] float timeToHoldSlam = 2f;

    #endregion

    #region //LASER BEAM VARIABLES
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool usingLaser = false;
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool isTrackingForLaser = true;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] LineRenderer laserBeam;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserSpawnPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserStartPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserEndPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserHitPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] float laserMoveTime;
    #endregion

    #region //SWEEP ATTACK VARIABLES
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool isSweeping = false;
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool trackPlayerForSweep = true;
    [FoldoutGroup("SweepAttack")] List<GameObject> playerSideDetectionColliders = new List<GameObject>();
    [FoldoutGroup("SweepAttack")][SerializeField] GameObject rightSweepAttackBoxCol2D;
    [FoldoutGroup("SweepAttack")][SerializeField] GameObject leftSweepAttackBoxCol2D;
    [FoldoutGroup("SweepAttack")][SerializeField] GameObject activeSweepAttackBoxCol2D;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform kaliBasePosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepPosition1;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepCurrentPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepPosition2;
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
        playerSideDetectionColliders.AddRange(new GameObject[] {rightSlamAttackBoxCol2D, leftSlamAttackBoxCol2D});
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
    //runs every frame and sets the player-occupied zone as the "active" slam zone
    private void SlamOnPlayerSide(GameObject side) 
    {  
        activeSlamAttackBoxCol2D = side.gameObject;
    }

    private void SweepOnPlayerSide(GameObject side) 
    {
        playerSideDetectionColliders.Remove(side.gameObject);
        
        if(playerSideDetectionColliders[0] == rightSlamAttackBoxCol2D) activeSweepAttackBoxCol2D = rightSweepAttackBoxCol2D;
        else if(playerSideDetectionColliders[0] == leftSlamAttackBoxCol2D) activeSweepAttackBoxCol2D = leftSweepAttackBoxCol2D;

        trackPlayerForSweep = false;
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
            else if(bossStage == BossStages.Stage2)Stage2CurrentState = S2BossStates.S2Attacking;
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            if(bossStage == BossStages.Stage1)Stage1CurrentState = S1BossStates.S1Idle;
            else if(bossStage == BossStages.Stage2)Stage2CurrentState = S2BossStates.S2Idle;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            if(bossStage == BossStages.Stage1)Stage1CurrentState = S1BossStates.S1Dead;
            else if(bossStage == BossStages.Stage2)Stage2CurrentState = S2BossStates.S2Dead;
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
                activeSlamAttackBoxCol2D.SendMessage("Slamming");
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
        float timeSinceStarted = 0f;

        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, sweepPosition1.position, timeSinceStarted);

            if (transform.position == sweepPosition1.position) // If the object has arrived...
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
        trackPlayerForSweep = false;

        float timeSinceStarted = 0f;

        while (true)
        {
            anim.SetBool("S2SweepAttack", true);

            timeSinceStarted += Time.deltaTime;
            transform.position = Vector3.Lerp(sweepPosition1.position, sweepPosition2.position, timeSinceStarted);

            if (transform.position == sweepPosition2.position) // If the object has arrived...
            {
                transform.position = Vector3.Lerp(transform.position, kaliBasePosition.position, timeSinceStarted);
                anim.SetBool("S2SweepAttack", false);
                activeSlamAttackBoxCol2D.SendMessage("Sweeping");   
                Stage2CurrentState = S2BossStates.S2Idle;
                trackPlayerForSweep = true;
                isSweeping = false;
                
                yield break; //...stop the coroutine
            }
           
            yield return null; // Otherwise, continue next frame
        }
    }

    IEnumerator LaserEyeBeam()
    {
        usingLaser = true;

        float sqrRemainingDistanceToDestination = (laserHitPosition.transform.position - laserEndPosition.transform.position).sqrMagnitude;
        float timeSinceStarted = 0f;

        while (true)
        {
            anim.SetBool("S2LaserEyeBeam", true);

            timeSinceStarted += Time.deltaTime;

            RaycastHit2D hit = Physics2D.Raycast(laserSpawnPosition.transform.position, laserHitPosition.transform.position);

            if(hit.collider){
                if(hit.collider.tag == "Obstacle")
                {
                    laserHitPosition.transform.position = new Vector3(hit.point.x, hit.point.y);
                }}

            laserBeam.SetPosition(0, laserSpawnPosition.transform.position); //defines 1st ("start") point
            laserBeam.SetPosition(1, laserHitPosition.transform.position); //defines 2nd (or "end") point
            
            laserHitPosition.transform.position = laserStartPosition.transform.position;
            laserHitPosition.transform.position = Vector3.Lerp(laserStartPosition.transform.position, laserEndPosition.transform.position, timeSinceStarted);

            if (laserHitPosition.transform.position == laserEndPosition.transform.position) // If the object has arrived...
            {
                anim.SetBool("S2LaserEyeBeam", false);
                usingLaser = false;  
                laserBeam.enabled = false;
                Stage2CurrentState = S2BossStates.S2Idle;
                yield break; //...stop the coroutine
            }
           
            yield return null; // Otherwise, continue next frame
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