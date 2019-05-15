using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class KaliBossAI : MonoBehaviour
{    
    #region //GENERAL VARIABLES
    [FoldoutGroup("General")] public Animator anim;
    public GameObject player;
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
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject rightAttackBoxCol2D;
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject leftAttackBoxCol2D;
    [FoldoutGroup("SlamAttack")][SerializeField] GameObject activeAttackBoxCol2D;
   
    [FoldoutGroup("SlamAttack")][SerializeField] int maxRandom;
    [FoldoutGroup("SlamAttack")][SerializeField] int minAttackValue;
    [FoldoutGroup("SlamAttack")][SerializeField] int randAttackValue;
    [FoldoutGroup("SlamAttack")][SerializeField] int attackProbabilityBooster;
    [FoldoutGroup("SlamAttack")][SerializeField] float timeHeld = 0;
    [FoldoutGroup("SlamAttack")][SerializeField] float timeToHold = 2f;
    [FoldoutGroup("SlamAttackDebug")][SerializeField]  bool isSlamming = false;
    [FoldoutGroup("SlamAttackDebug")][ShowInInspector] public static bool isTrackingPlayerSide = false;

    #endregion

    #region //LASER BEAM VARIABLES
    [FoldoutGroup("LaserBeamAttack")][SerializeField] LineRenderer laserBeam;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] Transform laserSpawn;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] Vector3 laserStartPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] Vector3 laserTargetPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] Vector3 laserCurrentPosition;
    [FoldoutGroup("LaserBeamAttack")][SerializeField] float laserMoveTime;
    #endregion

    #region //KALI ENUM STATES
    public enum BossStages {Stage1,Stage2,}

    public enum S1BossStates {S1Idle,S1Attacking,S1Dead,}

    public enum S2BossStates {S2Idle,S2Attacking,S2Dead,}
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

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
            StartCoroutine(StateSwitch());
        }

        //stateMachine.Update();

        WatchForStageChange();

        UpdateCurrentState();

        StartCoroutine(LaserEyeBeam());
    }

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

    public void StartChildCoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }

    //runs every frame and sets the player-occupied zone as the "active" slam zone
    private void SlamOnPlayerSide(GameObject side) 
    {        
        Debug.Log("Player is on " + side.gameObject.name);        
        activeAttackBoxCol2D = side.gameObject;
    }

    private void SweepOnPlayerSide(GameObject side) 
    {        
        Debug.Log("Player is on " + side.gameObject.name);        
        activeAttackBoxCol2D = side.gameObject;
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
                attackState = true;  StartCoroutine(SlamAttack());// SlamAttack();
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

    IEnumerator SlamAttack()
    {
        //anim.SetBool("S1SlamAttack", true);

        if(timeHeld >= timeToHold)
        {
            activeAttackBoxCol2D.SendMessage("Slamming");
            timeHeld = 0;
            Debug.Log("swtiching states");
            Stage1CurrentState = S1BossStates.S1Idle;            
            Stage2CurrentState = S2BossStates.S2Idle;
            yield break;
        }

        else timeHeld += Time.deltaTime;
        yield return null;   
    }

    IEnumerator SweepAttack()
    {
        
        yield break;
    }

    IEnumerator LaserEyeBeam()
    {
        laserTargetPosition = player.transform.position;

        RaycastHit2D hit = Physics2D.Linecast(laserStartPosition, laserTargetPosition);

        if(hit.collider){
            if(hit.collider.tag == "Obstacle")
            {
                laserTargetPosition = new Vector3(hit.point.x, hit.point.y);
                yield break;
            }}

        Debug.DrawLine(laserStartPosition, laserTargetPosition, Color.white);

        laserBeam.SetPosition(0, laserStartPosition); //defines 1st ("start") point
        laserBeam.SetPosition(1, laserTargetPosition); //defines 2nd (or "end") point
    }

    void AttackManager()
    {
        StartCoroutine(SlamAttack());
        StartCoroutine(SweepAttack());
        StartCoroutine(LaserEyeBeam());
    }

    void RandomizeAttacks()
    {

    }

    void WatchForStateChange()
    {
        
    }

    void WatchForStageChange()
    {
        if(lifePoints <= lifepointsToChangeState) bossStage = BossStages.Stage2;
        else return;
    }
}