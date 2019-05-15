﻿using Sirenix.OdinInspector;
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
    #endregion
    
    #region //SLAM ATTACK VARIABLES
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject rightSlamAttackBoxCol2D;
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject leftSlamAttackBoxCol2D;
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject activeSlamAttackBoxCol2D;   
    [FoldoutGroup("SlamAttack")][SerializeField] int maxRandom;
    [FoldoutGroup("SlamAttack")][SerializeField] int minAttackValue;
    [FoldoutGroup("SlamAttack")][SerializeField] int randAttackValue;
    [FoldoutGroup("SlamAttack")][SerializeField] int attackProbabilityBooster;
    [FoldoutGroup("SlamAttack")][SerializeField] float timeHoldingSlam = 0;
    [FoldoutGroup("SlamAttack")][SerializeField] float timeToHoldSlam = 2f;
    [FoldoutGroup("SlamAttackDebug")][SerializeField]  public static bool isSlamming = false;
    [FoldoutGroup("SlamAttackDebug")][ShowInInspector] public static bool isTrackingForSlam = true;

    #endregion

    #region //LASER BEAM VARIABLES
    [FoldoutGroup("LaserBeamAttack")][SerializeField] LineRenderer laserBeam;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserSpawnPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserStartPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserEndPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] GameObject laserHitPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] float laserMoveTime;
    [FoldoutGroup("LaserBeamAttackDebug")][SerializeField] bool usingLaser = false;
    #endregion

    #region //SWEEP ATTACK VARIABLES
    [FoldoutGroup("SweepAttack")] List<GameObject> playerSideDetectionColliders = new List<GameObject>();
    [FoldoutGroup("SweepAttack")][SerializeField] GameObject rightSweepAttackBoxCol2D;
    [FoldoutGroup("SweepAttack")][SerializeField] GameObject leftSweepAttackBoxCol2D;
    [FoldoutGroup("SweepAttack")][SerializeField] GameObject activeSweepAttackBoxCol2D;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepStartPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepCurrentPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] Transform sweepDestinationPosition;
    [FoldoutGroup("SweepAttack")][SerializeField] float timeHoldingSweep = 0;
    [FoldoutGroup("SweepAttack")][SerializeField] float timeToHoldSweep = 2f;
    [FoldoutGroup("SweepAttackDebug")][SerializeField] public static bool isSweeping = false;
    [FoldoutGroup("SweepAttackDebug")][SerializeField] public static bool trackPlayerForSweep = true;
    #endregion

    #region //STATE CHANGE VARIABLES
    [FoldoutGroup("ChangeStateVariables")][SerializeField] float minWaitTime;
    [FoldoutGroup("ChangeStateVariables")][SerializeField] float maxWaitTime;
    [FoldoutGroup("ChangeStateVariables")][SerializeField] float currentRunTime;
    [FoldoutGroup("ChangeStateVariables")][SerializeField] float timeTillNextAttack;
    [FoldoutGroup("ChangeStateVariables")][SerializeField] float probabilityBooster;
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
            Stage2CurrentState = S2BossStates.S2Attacking;
        }

        //stateMachine.Update();

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

    IEnumerator Stage1StateManager()
    {
        timeTillNextAttack = Random.Range (minWaitTime, maxWaitTime);

        while (Stage1CurrentState == S1BossStates.S1Attacking)
        {
            if (currentRunTime == timeTillNextAttack)
            {
                StartCoroutine(SlamAttack());
                currentRunTime = 0;
            }
            else currentRunTime += Time.deltaTime;
            yield break;
        }

        yield return null;

        //if(Stage1CurrentState == S1BossStates.S1Idle) StartCoroutine(IdleState());
        //if(Stage1CurrentState == S1BossStates.S1Dead) StartCoroutine(DeathState()); 
    }

    IEnumerator Stage2StateManager()
    {
        timeTillNextAttack = Random.Range (minWaitTime, maxWaitTime);

        while (Stage2CurrentState == S2BossStates.S2Attacking)
        {
            if (currentRunTime == timeTillNextAttack)
            {
                RandomizeAttacks();
                currentRunTime = 0;
            }
            else currentRunTime += Time.deltaTime;
            yield break;
        }

        yield return null;

        //if(Stage2CurrentState == S2BossStates.S2Idle) StartCoroutine(IdleState());
        //if(Stage2CurrentState == S2BossStates.S2Dead) StartCoroutine(DeathState());
    }

    void RandomizeAttacks()
    {
        int whichAttack  = Random.Range(0, 3);

        Debug.Log("I'm attacking with " + whichAttack);

        if(whichAttack == 0) StartCoroutine(SlamAttack());
        if(whichAttack == 1) StartCoroutine(SweepAttack());
        if(whichAttack == 2) StartCoroutine(LaserEyeBeam());
    }
    #endregion

    #region //STATE CHANGING
    IEnumerator StateSwitch()
    {
        randAttackValue = Random.Range(0, maxRandom);

        Debug.Log(randAttackValue);

        if(bossStage == BossStages.Stage1)
        {
            if(randAttackValue >= minAttackValue) minAttackValue += attackProbabilityBooster; //does not attack, raise probability nothing happens
            else if(randAttackValue <= minAttackValue) Stage1CurrentState = S1BossStates.S1Attacking; //attacks, repeat
        }

        if(bossStage == BossStages.Stage2)
        {
            if(randAttackValue >= minAttackValue) minAttackValue += attackProbabilityBooster; //does not attack, raise probability nothing happens
            else if(randAttackValue <= minAttackValue) Stage2CurrentState = S2BossStates.S2Attacking; //attacks, repeat
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
        yield break; //exit the coroutine
    }

    IEnumerator SweepAttack()
    {
        isSweeping = true;
        trackPlayerForSweep = false;

        float sqrRemainingDistanceToDestination = (transform.position - sweepDestinationPosition.position).sqrMagnitude;
        float timeSinceStarted = 0f;

        while (true)
        {
            anim.SetBool("S2SweepAttack", true);

            timeSinceStarted += Time.deltaTime;
            transform.position = Vector3.Lerp(sweepStartPosition.position, sweepDestinationPosition.position, timeSinceStarted);

            if (sqrRemainingDistanceToDestination <= float.Epsilon) // If the object has arrived...
            {
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

        laserHitPosition.transform.position = laserStartPosition.transform.position;

        float sqrRemainingDistanceToDestination = (laserHitPosition.transform.position - laserEndPosition.transform.position).sqrMagnitude;
        float timeSinceStarted = 0f;

        while (true)
        {
            anim.SetBool("S2LaserEyeBeam", true);

            timeSinceStarted += Time.deltaTime;
            laserHitPosition.transform.position = Vector3.Lerp(laserStartPosition.transform.position, laserEndPosition.transform.position, timeSinceStarted);

            RaycastHit2D hit = Physics2D.Raycast(laserSpawnPosition.transform.position, laserHitPosition.transform.position);

            if(hit.collider){
                if(hit.collider.tag == "Obstacle")
                {
                    laserHitPosition.transform.position = new Vector3(hit.point.x, hit.point.y);
                }}

            laserBeam.SetPosition(0, laserSpawnPosition.transform.position); //defines 1st ("start") point
            laserBeam.SetPosition(1, laserHitPosition.transform.position); //defines 2nd (or "end") point

            if (sqrRemainingDistanceToDestination <= float.Epsilon) // If the object has arrived...
            {
                anim.SetBool("S2LaserEyeBeam", false);
                usingLaser = false;  
                Stage2CurrentState = S2BossStates.S2Idle;
                yield break; //...stop the coroutine
            }
           
            yield return null; // Otherwise, continue next frame
        }
    }
    #endregion

    #region //IDLE_OTHER STATES
    #endregion
}