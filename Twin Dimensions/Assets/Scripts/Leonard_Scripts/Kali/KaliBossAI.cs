using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KaliBossAI : SerializedMonoBehaviour
{ 
    #region Variable Declarations
    #region //GENERAL
    [FoldoutGroup("General")][SerializeField] Animator anim;
    [FoldoutGroup("General")][SerializeField] GameObject player;
    [FoldoutGroup("General")][SerializeField] GameObject kaliStatue;
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
    [FoldoutGroup("PlayerTrackingDebug")][SerializeField] GameObject leftMapDetectionCollider;
    [FoldoutGroup("PlayerTrackingDebug")][SerializeField] GameObject rightMapDetectionCollider;
    [FoldoutGroup("PlayerTrackingDebug")][SerializeField] GameObject currentPlayerActiveSideCollider;
    [FoldoutGroup("PlayerTrackingDebug")][ShowInInspector] public static bool isTrackingPlayerPosition = true;
    #endregion
    
    #region //SLAM ATTACK
    [FoldoutGroup("AttackDebug")][ShowInInspector]  public static bool isSlamming = false;
    [FoldoutGroup("AttackVariablesDebug")][SerializeField] float timeHoldingSlam = 0;
    [FoldoutGroup("AttackVariablesDebug")][SerializeField] float timeToHoldSlam = 2f;
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject slamLeftCollider;
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject slamRightCollider;
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject activeSlamSide;
    #endregion

    #region //LASER BEAM
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool usingLaser = false;
    [FoldoutGroup("AttackVariablesDebug")][SerializeField] float timeHoldingLaser = 0f;
    [FoldoutGroup("AttackVariablesDebug")][SerializeField] float timeToHoldLaser = 1.75f;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] LineRenderer laserBeam;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserLeftPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserRightPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserSpawnPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserHitPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserStartPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserEndPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] float laserMoveTime;
    #endregion

    #region //SWEEP ATTACK
    [FoldoutGroup("AttackDebug")][ShowInInspector] public static bool isSweeping = false;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform kaliBasePosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepLeftPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepRightPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepStartPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepEndPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepCurrentPosition;
    [FoldoutGroup("AttackVariablesDebug")][SerializeField] float timeHoldingSweep = 0f;
    [FoldoutGroup("AttackVariablesDebug")][SerializeField] float timeToHoldSweep = 2f;
    #endregion

    #region //STATE CHANGE
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
    }

    // Update is called once per frame
    void Update()
    {
        //Debugging, used to test attacking Kali
        if(Input.GetKeyDown(KeyCode.F))
        {
            lifePoints -= damageValue;
            StartCoroutine(StateSwitch(4));
        }

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

        activeSlamSide = side.gameObject;

        if (activeSlamSide == rightMapDetectionCollider)
        {
            anim.SetBool("slamLeft", true);
            anim.SetBool("slamRight", false);
            
            // slamRightCollider.gameObject.SetActive(true);
            // slamLeftCollider.gameObject.SetActive(false);
        }

        else if (activeSlamSide == leftMapDetectionCollider)
        {
            anim.SetBool("slamRight", true);
            anim.SetBool("slamLeft", false);
            
            //slamLeftCollider.gameObject.SetActive(true);
            //slamRightCollider.gameObject.SetActive(false);
        }

        if(currentPlayerActiveSideCollider == rightMapDetectionCollider)
        {
            laserStartPosition = laserLeftPosition; //if player is on the right, laser from left
            laserEndPosition = laserRightPosition;
        }

        else if(currentPlayerActiveSideCollider == leftMapDetectionCollider)
        {
            laserStartPosition = laserRightPosition; //if player is on the left, laser from right
            laserEndPosition = laserLeftPosition;
        }

        if(currentPlayerActiveSideCollider == rightMapDetectionCollider)
        {
            sweepStartPosition = sweepLeftPosition; //if player is on the right, sweep from left
            sweepEndPosition = sweepRightPosition;
            anim.SetBool("sweepRight", true);
            anim.SetBool("sweepLeft", false);
        }

        else if(currentPlayerActiveSideCollider == leftMapDetectionCollider)
        {
            sweepStartPosition = sweepRightPosition; //if player is on the left, sweep from right
            sweepEndPosition = sweepLeftPosition;  
            anim.SetBool("sweepLeft", true);
            anim.SetBool("sweepRight", false);
        }
    }
    #endregion
    
    #region //ATTACK MANAGER
    void WatchForStageChange()
    {
        if(lifePoints <= lifepointsToChangeState){bossStage = BossStages.Stage2; anim.SetTrigger("isTransitioning");}
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

    #region //STATS MANAGEMENT
    void DealDamage(string damageType)
    {
        switch (damageType)
        {
            case "Elephant": Debug.Log("An Elephant Hit Me"); lifePoints = lifePoints - damageValue; break;

            case "Yeetos": Debug.Log("A Yeetos Hit Me"); break;

            default: Debug.Log("Wut in a bugger's dick is this?"); break;
        }
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
        isTrackingPlayerPosition = false;
        timeHoldingSlam = 0;

        while(true)
        {
            anim.SetBool("SlamAttack", true);
            
            timeHoldingSlam += Time.deltaTime;

            if(timeHoldingSlam >= timeToHoldSlam) // If Kali has held long enough...
            {
                if (activeSlamSide == rightMapDetectionCollider)
                {
                    slamRightCollider.gameObject.SetActive(true);
                    slamLeftCollider.gameObject.SetActive(false);
                }

                if (activeSlamSide == leftMapDetectionCollider)
                {
                    slamLeftCollider.gameObject.SetActive(true);
                    slamRightCollider.gameObject.SetActive(false);
                }

                anim.SetBool("SlamAttack", false);
                currentPlayerActiveSideCollider.SendMessage("Slamming");

                Stage1CurrentState = S1BossStates.S1Idle;
                Stage2CurrentState = S2BossStates.S2Idle;

                isTrackingPlayerPosition = true;
                isSlamming = false;

                yield return new WaitForSeconds(2);
                slamLeftCollider.gameObject.SetActive(false);
                slamRightCollider.gameObject.SetActive(false);
                yield break; //...stop the coroutine
            }
            
            yield return null; // Otherwise, continue next frame
        }
    }
    
    IEnumerator MoveToSweepAttackLocation()
    {
        isTrackingPlayerPosition = false;
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
        timeHoldingSweep = 0f;

        float timeSinceStarted = 0f;

        while (true)
        {
            timeHoldingSweep += Time.deltaTime;
            anim.SetBool("SweepAttack", true);

            if (timeHoldingSweep >= timeToHoldSweep)
            {
                timeSinceStarted += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, sweepEndPosition.position, timeSinceStarted * 0.5f);

                if (transform.position == sweepEndPosition.position) // If the object has arrived...
                {
                    transform.position = Vector3.Lerp(transform.position, kaliBasePosition.position, 1f);

                    if (transform.position == kaliBasePosition.position)
                    {                      
                        anim.SetBool("SweepAttack", false);
                        
                        currentPlayerActiveSideCollider.SendMessage("Sweeping");
                        
                        Stage2CurrentState = S2BossStates.S2Idle;
                        
                        isTrackingPlayerPosition = true;
                        isSweeping = false;
                        yield break; //...stop the coroutine  
                    }

                    yield return null;
                }

                yield return null; // Otherwise, continue next frame
            }

            yield return null;
        }
    }

    public IEnumerator LaserEyeBeam()
    {
        isTrackingPlayerPosition = false;        
        timeHoldingLaser = 0;

        float sqrRemainingDistanceToDestination = (laserHitPosition.transform.position - laserEndPosition.transform.position).sqrMagnitude;
        float timeSinceStarted = 0f;

        while (true)
        {
            timeHoldingLaser += Time.deltaTime;
            anim.SetBool("LaserAttack", true);

            if (timeHoldingLaser >= timeToHoldLaser)
            {                
                laserBeam.enabled = true;
                usingLaser = true;
                timeSinceStarted += Time.deltaTime;

                RaycastHit2D hit = Physics2D.Raycast(laserSpawnPosition.transform.position, laserHitPosition.transform.position);
                Debug.DrawRay(laserSpawnPosition.transform.position, laserHitPosition.transform.position, Color.green);
                if (hit.collider)
                {
                    Debug.Log("I hit " + hit.collider.name);

                    if (hit.collider.tag == "Player")
                    {
                        Debug.Log("Have touched the player");
                        //laserHitPosition.transform.position = new Vector3(hit.point.x, hit.point.y);
                    }

                    if(hit.collider.tag == "Obstacle")
                    {
                        Debug.Log("I hit " + hit.collider.name);
                    }

                    if(hit.collider.tag == "Enemy")
                    {
                        Debug.Log("I hit " + hit.collider.name);
                    }
                }

                laserHitPosition.transform.position = laserStartPosition.transform.position;
                laserHitPosition.transform.position = Vector3.Lerp(laserStartPosition.transform.position, laserEndPosition.transform.position, timeSinceStarted);

                laserBeam.SetPosition(0, laserSpawnPosition.transform.position); //defines 1st ("start") point
                laserBeam.SetPosition(1, laserHitPosition.transform.position); //defines 2nd (or "end") point

                if (laserHitPosition.transform.position == laserEndPosition.transform.position) // If the object has arrived...
                {
                    anim.SetBool("LaserAttack", false);
                    usingLaser = false;
                    laserBeam.enabled = false;
                    isTrackingPlayerPosition = true;
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