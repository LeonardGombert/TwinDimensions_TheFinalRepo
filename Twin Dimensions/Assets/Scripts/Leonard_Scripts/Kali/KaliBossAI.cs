using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class KaliBossAI : MonoBehaviour
{
    public StateMachine<KaliBossAI> stateMachine { get; set; }
    
    #region //GENERAL VARIABLES
    [FoldoutGroup("General")] public Animator anim;
    #endregion

    #region //BOSS STATES AND STAGES
    [HideInInspector] public S1BossStates defautState = S1BossStates.S1Idle;
    [FoldoutGroup("KaliStats")] public BossStages bossStage;
    [FoldoutGroup("KaliStats")] public S1BossStates Stage1CurrentState;
    [FoldoutGroup("KaliStats")] public S2BossStates Stage2CurrentState;
    [FoldoutGroup("KaliDebug")] public bool attackState = false;
    [FoldoutGroup("KaliDebug")] public bool idleState = false;
    [FoldoutGroup("KaliDebug")] public bool deathState = false;
    #endregion
    
    #region //KALI STATS
    [FoldoutGroup("KaliStats")] [SerializeField] float lifePoints;
    [FoldoutGroup("KaliStats")] [SerializeField] float lifepointsToChangeState;
    [FoldoutGroup("KaliStats")] [SerializeField] float damageValue;
    #endregion
    
    #region //SLAM ATTACK VARIABLES
    [FoldoutGroup("SlamAttack")] public GameObject rightAttackBoxCol2D;
    [FoldoutGroup("SlamAttack")] public GameObject leftAttackBoxCol2D;
    [FoldoutGroup("SlamAttack")] public GameObject activeAttackBoxCol2D;
   
    [FoldoutGroup("SlamAttack")] public int maxRandom;
    [FoldoutGroup("SlamAttack")] public int minAttackValue;
    [FoldoutGroup("SlamAttack")] public int randAttackValue;
    [FoldoutGroup("SlamAttack")] public int attackProbabilityBooster;
    [FoldoutGroup("SlamAttackDebug")] public bool isSlamming = false;
    [FoldoutGroup("SlamAttackDebug")][ShowInInspector] public static bool isTrackingPlayerSide = false;
    #endregion

    #region //LASER BEAM VARIABLES
    [FoldoutGroup("LaserBeamAttack")] public LineRenderer laserBeam;
    [FoldoutGroup("LaserBeamAttack")] public Transform laserSpawn;
    [FoldoutGroup("LaserBeamAttack")] public Transform laserTarget;
    [FoldoutGroup("LaserBeamAttack")] public List<Transform> laserPointList = new List<Transform>();
    #endregion

    #region //KALI ENUM STATES
    public enum BossStages
    {
        Stage1,
        Stage2,
    }

    public enum S1BossStates
    {
        S1Idle,
        S1Attacking,
        S1Dead,
    }

    public enum S2BossStates
    {
        S2Idle,
        S2Attacking, 
        S2Dead, 
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        stateMachine = new StateMachine<KaliBossAI>(this);
        stateMachine.ChangeState(IdleState.Instance);
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

        stateMachine.Update();

        WatchForStageChange();

        UpdateCurrentState();

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

    void WatchForStageChange()
    {
        if(lifePoints <= lifepointsToChangeState)
        {
            bossStage = BossStages.Stage2;
            stateMachine.ChangeState(GeneralAttackStateManager.Instance);
        }

        else return;
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

        if(bossStage == BossStages.Stage2)
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
}