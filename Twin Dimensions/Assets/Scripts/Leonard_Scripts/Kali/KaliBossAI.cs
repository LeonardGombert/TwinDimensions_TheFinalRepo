using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KaliBossAI : MonoBehaviour
{
    #region Variable Declarations
    #region //GENERAL
    [FoldoutGroup("General")] [SerializeField] Animator anim;
    [FoldoutGroup("General")] [SerializeField] GameObject player;
    [FoldoutGroup("General")] [SerializeField] GameObject kaliStatue;
    #endregion

    #region //BOSS STATES AND STAGES
    S1BossStates defautState = S1BossStates.S1Idle;
    [FoldoutGroup("KaliStats")] [SerializeField] BossStages bossStage;
    [FoldoutGroup("KaliStats")] [SerializeField] S1BossStates Stage1CurrentState;
    [FoldoutGroup("KaliStats")] [SerializeField] S2BossStates Stage2CurrentState;
    [FoldoutGroup("KaliDebug")] [SerializeField] bool attackState = false;
    [FoldoutGroup("KaliDebug")] [SerializeField] bool idleState = false;
    [FoldoutGroup("KaliDebug")] [SerializeField] bool deathState = false;
    #endregion

    #region //KALI STATS
    [FoldoutGroup("KaliStats")] [SerializeField] float lifePoints;
    [FoldoutGroup("KaliStats")] [SerializeField] float lifepointsToChangeState;
    [FoldoutGroup("KaliStats")] [SerializeField] float damageValue;
    [FoldoutGroup("KaliDebug")] [SerializeField] bool damageDealt = false;
    [FoldoutGroup("PlayerTrackingDebug")] [SerializeField] GameObject leftMapDetectionCollider;
    [FoldoutGroup("PlayerTrackingDebug")] [SerializeField] GameObject rightMapDetectionCollider;
    [FoldoutGroup("PlayerTrackingDebug")] [SerializeField] GameObject currentPlayerActiveSideCollider;
    [FoldoutGroup("PlayerTrackingDebug")] [ShowInInspector] public static bool isTrackingPlayerPosition = true;
    #endregion

    #region //SLAM ATTACK
    [FoldoutGroup("AttackDebug")] [ShowInInspector] public static bool isSlamming = false;
    [FoldoutGroup("AttackVariablesDebug")] [SerializeField] float timeHoldingSlam = 0;
    [FoldoutGroup("AttackVariablesDebug")] [SerializeField] float timeToHoldSlam = 2f;
    [FoldoutGroup("AttackUpdatesDebug")] [SerializeField] GameObject activeSlamSide;
    [FoldoutGroup("SlamAttack")] [SerializeField] GameObject slamLeftCollider;
    [FoldoutGroup("SlamAttack")] [SerializeField] GameObject slamRightCollider;

    #endregion

    #region //LASER BEAM
    [FoldoutGroup("AttackDebug")] [ShowInInspector] public static bool usingLaser = false;
    [FoldoutGroup("AttackVariablesDebug")] [SerializeField] float timeHoldingLaser = 0f;
    [FoldoutGroup("AttackVariablesDebug")] [SerializeField] float timeToHoldLaser = 1.75f;
    [FoldoutGroup("LaserBeamAttack")] [SerializeField] LineRenderer laserBeam;
    [FoldoutGroup("LaserBeamAttack")] [SerializeField] GameObject laserLeftPosition;
    [FoldoutGroup("LaserBeamAttack")] [SerializeField] GameObject laserRightPosition;
    [FoldoutGroup("LaserBeamAttack")] [SerializeField] GameObject laserSpawnPosition;
    [FoldoutGroup("LaserBeamAttack")] [SerializeField] GameObject laserHitPosition;
    [FoldoutGroup("AttackUpdatesDebug")] [SerializeField] GameObject laserStartPosition;
    [FoldoutGroup("AttackUpdatesDebug")] [SerializeField] GameObject laserEndPosition;
    [FoldoutGroup("LaserBeamAttack")] [SerializeField] float laserMoveTime;
    #endregion

    #region //SWEEP ATTACK
    [FoldoutGroup("AttackDebug")] [ShowInInspector] public static bool isSweeping = false;
    [FoldoutGroup("SweepAttack")] [SerializeField] Vector3 kaliBasePosition;
    [FoldoutGroup("SweepAttack")] [SerializeField] Transform sweepLeftPosition;
    [FoldoutGroup("SweepAttack")] [SerializeField] Transform sweepRightPosition;
    [FoldoutGroup("SweepAttack")] [SerializeField] GameObject sweepLeftCollider;
    [FoldoutGroup("SweepAttack")] [SerializeField] GameObject sweepRightCollider;
    [FoldoutGroup("SweepAttack")] [SerializeField] GameObject activeSweepSide;
    [FoldoutGroup("AttackUpdatesDebug")] [SerializeField] Transform sweepStartPosition;
    [FoldoutGroup("AttackUpdatesDebug")] [SerializeField] Transform sweepEndPosition;
    [FoldoutGroup("AttackUpdatesDebug")] [SerializeField] Transform sweepCurrentPosition;
    [FoldoutGroup("AttackVariablesDebug")] [SerializeField] float timeHoldingSweep = 0f;
    [FoldoutGroup("AttackVariablesDebug")] [SerializeField] float timeToHoldSweep = 2f;
    #endregion

    #region //STATE CHANGE
    [FoldoutGroup("ChangeStateDebug")] [SerializeField] bool newAttackTimeGenerated = false;
    [FoldoutGroup("ChangeStateDebug")] [SerializeField] float stage1TimeLeftBeforeAttack;
    [FoldoutGroup("ChangeStateDebug")] [SerializeField] float stage1MaxTimeBeforeAttack;
    [FoldoutGroup("ChangeStateDebug")] [SerializeField] float stage1CurrentRunTimeBeforeAttack;

    [FoldoutGroup("ChangeStateDebug")] [SerializeField] float stage2TimeBeforeAttack;
    [FoldoutGroup("ChangeStateDebug")] [SerializeField] float stage2MinTimeBeforeAttack;
    [FoldoutGroup("ChangeStateDebug")] [SerializeField] float stage2MaxTimeBeforeAttack;
    [FoldoutGroup("ChangeStateDebug")] [SerializeField] float stage2CurrentRunTimeBeforeAttack;
    #endregion

    #region //KALI ENUM STATES
    public enum BossStages { Stage1, Stage2, }

    public enum S1BossStates { S1Idle, S1Attacking, S1Dead, }

    public enum S2BossStates { S2Idle, S2Attacking, S2Dead, }
    #endregion
    #endregion

    #region Monobehavior Callbacks
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        sweepCurrentPosition = this.gameObject.transform;
        kaliBasePosition = this.gameObject.transform.position;

        slamLeftCollider.gameObject.SetActive(false);
        slamRightCollider.gameObject.SetActive(false);

        sweepLeftCollider.gameObject.SetActive(false);
        sweepRightCollider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (bossStage == BossStages.Stage1 && !newAttackTimeGenerated) StartCoroutine(Stage1StateManager());
        if (bossStage == BossStages.Stage2 && !newAttackTimeGenerated) StartCoroutine(Stage2StateManager());

        WatchForStageChange();

        UpdateCurrentState();

        DebugStateSwitching();
    }
    #endregion

    #region //CHECK PLAYER SIDE
    private void UpdatePlayerSide(GameObject side)
    {
        currentPlayerActiveSideCollider = side.gameObject;

        activeSlamSide = side.gameObject;
        activeSweepSide = side.gameObject;

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

        if (currentPlayerActiveSideCollider == rightMapDetectionCollider)
        {
            laserStartPosition = laserLeftPosition; //if player is on the right, laser from left
            laserEndPosition = laserRightPosition;
        }

        else if (currentPlayerActiveSideCollider == leftMapDetectionCollider)
        {
            laserStartPosition = laserRightPosition; //if player is on the left, laser from right
            laserEndPosition = laserLeftPosition;
        }

        if (currentPlayerActiveSideCollider == rightMapDetectionCollider)
        {
            sweepStartPosition = sweepLeftPosition; //if player is on the right, sweep from left
            sweepEndPosition = sweepRightPosition;
            anim.SetBool("sweepRight", true);
            anim.SetBool("sweepLeft", false);
        }

        else if (currentPlayerActiveSideCollider == leftMapDetectionCollider)
        {
            sweepStartPosition = sweepRightPosition; //if player is on the left, sweep from right
            sweepEndPosition = sweepLeftPosition;
            anim.SetBool("sweepLeft", true);
            anim.SetBool("sweepRight", false);
        }
    }
    #endregion

    #region //STATE ATTACK MANAGERS
    IEnumerator Stage1StateManager()
    {
        stage1CurrentRunTimeBeforeAttack = 0;
        //timeLeftBeforeAttack = Random.Range(0, maxTimeBeforeAttack);
        newAttackTimeGenerated = true;
        Debug.Log(stage1TimeLeftBeforeAttack);

        while (true)
        {
            if (Stage1CurrentState != S1BossStates.S1Attacking)
            {
                Stage1CurrentState = S1BossStates.S1Idle;

                stage1CurrentRunTimeBeforeAttack += Time.deltaTime;

                if (stage1CurrentRunTimeBeforeAttack >= stage1MaxTimeBeforeAttack)
                {
                    newAttackTimeGenerated = false;
                    RandomizeState1Attacks();
                    Debug.Log("Countdown complete, time to attack");
                    yield break;
                }
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator Stage2StateManager()
    {
        stage2CurrentRunTimeBeforeAttack = 0;
        stage2TimeBeforeAttack = Random.Range(stage2MinTimeBeforeAttack, stage2MaxTimeBeforeAttack);
        newAttackTimeGenerated = true;
        Debug.Log(stage1TimeLeftBeforeAttack);

        while (true)
        {
            if (Stage2CurrentState != S2BossStates.S2Attacking)
            {
                Stage2CurrentState = S2BossStates.S2Idle;

                stage2CurrentRunTimeBeforeAttack += Time.deltaTime;

                if (stage2CurrentRunTimeBeforeAttack >= stage2TimeBeforeAttack)
                {
                    newAttackTimeGenerated = false;
                    //RandomizeState2Attacks();
                    RandomizeState2Attacks();
                    Debug.Log("Countdown complete, time to attack");
                    yield break;
                }
                yield return null;
            }
            yield return null;
        }
    }

    void RandomizeState1Attacks()
    {
        int whichAttack = Random.Range(0, 2);

        Debug.Log("I'm attacking with " + whichAttack);

        if (whichAttack == 0) StartCoroutine(SlamAttack());
        if (whichAttack == 1) StartCoroutine(MoveToSweepAttackLocation());

        Stage1CurrentState = S1BossStates.S1Attacking;
    }

    void RandomizeState2Attacks()
    {
        int whichAttack = Random.Range(0, 3);

        Debug.Log("I'm attacking with " + whichAttack);

        if (whichAttack == 0) StartCoroutine(SlamAttack());
        if (whichAttack == 1) StartCoroutine(MoveToSweepAttackLocation());
        if (whichAttack == 2) StartCoroutine(LaserEyeBeam());

        Stage2CurrentState = S2BossStates.S2Attacking;
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
    void WatchForStageChange()
    {
        if (lifePoints <= lifepointsToChangeState) { bossStage = BossStages.Stage2; anim.SetTrigger("isTransitioning"); }
        else return;
    }

    void DebugStateSwitching()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (bossStage == BossStages.Stage1) Stage1CurrentState = S1BossStates.S1Attacking;
            else if (bossStage == BossStages.Stage2) StartCoroutine(LaserEyeBeam()); //Stage2CurrentState = S2BossStates.S2Attacking;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (bossStage == BossStages.Stage1) StartCoroutine(MoveToSweepAttackLocation());
            else if (bossStage == BossStages.Stage2) StartCoroutine(MoveToSweepAttackLocation()); //Stage2CurrentState = S2BossStates.S2Idle;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (bossStage == BossStages.Stage1) StartCoroutine(SlamAttack());
            else if (bossStage == BossStages.Stage2) StartCoroutine(SlamAttack()); //Stage2CurrentState = S2BossStates.S2Dead;
        }
    }

    /*void StateSwitch(float timeTillStateChange)
    {
        currentRunTime = 0;
        currentRunTime += Time.deltaTime;

        if (bossStage == BossStages.Stage1)
        {
            if (currentRunTime <= timeTillStateChange)
            {
                currentRunTime += attackProbabilityBooster; //does not attack, raise probability nothing happens
            }

            else if (currentRunTime >= timeTillStateChange)
            {
                Stage1CurrentState = S1BossStates.S1Attacking; //attacks, repeat
            }
        }

        if (bossStage == BossStages.Stage2)
        {
            if (currentRunTime <= timeTillStateChange)
            {
                currentRunTime += attackProbabilityBooster; //does not attack, raise probability nothing happens
            }
            else if (timeTillStateChange >= currentRunTime)
            {
                Stage2CurrentState = S2BossStates.S2Attacking; //attacks, repeat
            }
        }
    }*/

    void UpdateCurrentState()
    {
        if (bossStage == BossStages.Stage1)
        {
            switch (Stage1CurrentState)
            {
                case S1BossStates.S1Idle:
                    attackState = false;
                    idleState = true;
                    deathState = false;
                    break;

                case S1BossStates.S1Dead:
                    attackState = false;
                    idleState = false;
                    deathState = true;
                    break;

                case S1BossStates.S1Attacking:
                    attackState = true;
                    idleState = false;
                    deathState = false;
                    break;

                default:
                    Debug.Log("null");
                    break;
            }
        }

        else if (bossStage == BossStages.Stage2)
        {
            switch (Stage2CurrentState)
            {
                case S2BossStates.S2Idle:
                    attackState = false;
                    idleState = true;
                    deathState = false;
                    break;

                case S2BossStates.S2Dead:
                    attackState = false;
                    idleState = false;
                    deathState = true;
                    break;

                case S2BossStates.S2Attacking:
                    attackState = true;
                    idleState = false;
                    deathState = false;
                    break;

                default:
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

        while (true)
        {
            anim.SetBool("SlamAttack", true);

            timeHoldingSlam += Time.deltaTime;

            if (timeHoldingSlam >= timeToHoldSlam) // If Kali has held long enough...
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

                yield return new WaitForSeconds(.5f);
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
                if (activeSweepSide == rightMapDetectionCollider)
                {
                    sweepRightCollider.gameObject.SetActive(false);
                    sweepLeftCollider.gameObject.SetActive(true);
                }

                if (activeSweepSide == leftMapDetectionCollider)
                {
                    sweepLeftCollider.gameObject.SetActive(false);
                    sweepRightCollider.gameObject.SetActive(true);
                }

                timeSinceStarted += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, sweepEndPosition.position, timeSinceStarted * 0.5f);

                if (transform.position == sweepEndPosition.position) // If the object has arrived...
                {
                    transform.position = Vector3.Lerp(transform.position, kaliBasePosition, 1f);

                    if (transform.position == kaliBasePosition)
                    {
                        anim.SetBool("SweepAttack", false);

                        currentPlayerActiveSideCollider.SendMessage("Sweeping");

                        Stage1CurrentState = S1BossStates.S1Idle;
                        Stage2CurrentState = S2BossStates.S2Idle;

                        isTrackingPlayerPosition = true;
                        isSweeping = false;
                        sweepRightCollider.gameObject.SetActive(false);
                        sweepLeftCollider.gameObject.SetActive(false);
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
                sweepRightCollider.gameObject.SetActive(false);
                sweepLeftCollider.gameObject.SetActive(false);
                slamLeftCollider.gameObject.SetActive(false);
                slamRightCollider.gameObject.SetActive(false);
                leftMapDetectionCollider.gameObject.SetActive(false);
                rightMapDetectionCollider.gameObject.SetActive(false);

                laserBeam.enabled = true;
                usingLaser = true;
                timeSinceStarted += Time.deltaTime;

                RaycastHit2D hit = Physics2D.Linecast(laserSpawnPosition.transform.position, laserHitPosition.transform.position);
            
                // if (hit.collider)
                // {
                //     if (hit.collider.tag == "Player")
                //     {
                //         Debug.Log("Have touched the player");
                //     }

                //     if (hit.collider.tag == "Obstacle")
                //     {
                //         Debug.Log("I hit " + hit.collider.name);
                //         laserHitPosition.transform.position = new Vector3(hit.point.x, hit.point.y);
                //     }

                //     if (hit.collider.tag == "Enemy")
                //     {
                //         Debug.Log("I hit " + hit.collider.name);
                //     }
                // }
                
                Debug.DrawLine(laserSpawnPosition.transform.position, laserHitPosition.transform.position, Color.green);

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

                    sweepRightCollider.gameObject.SetActive(true);
                    sweepLeftCollider.gameObject.SetActive(true);
                    slamLeftCollider.gameObject.SetActive(true);
                    slamRightCollider.gameObject.SetActive(true);
                    leftMapDetectionCollider.gameObject.SetActive(true);
                    rightMapDetectionCollider.gameObject.SetActive(true);
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