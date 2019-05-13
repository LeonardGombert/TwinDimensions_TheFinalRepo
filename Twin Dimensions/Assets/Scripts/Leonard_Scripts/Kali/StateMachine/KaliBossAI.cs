using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class KaliBossAI : MonoBehaviour
{
    public S1BossStates defautState = S1BossStates.S1Idle;

    public BossStages bossStage;

    public S1BossStates Stage1CurrentState;
    public S2BossStates Stage2CurrentState;
    
    public Animator anim;
    
    public StateMachine<KaliBossAI> stateMachine { get; set; }
    public bool attackState = false;
    public bool idleState = false;
    public bool deathState = false;

    public bool isSlamming = false;
    public static bool isTrackingPlayerSide = false;

    [SerializeField] float lifePoints = 1000;
    [SerializeField] float lifepointsToChangeState = 750;
    [SerializeField] float damageValue = 10;
    [SerializeField] float previousLifepoints;
    
    #region //SLAM ATTACK
    [FoldoutGroup("Slam Attack")] public GameObject rightAttackBoxCol2D;
    [FoldoutGroup("Slam Attack")] public GameObject leftAttackBoxCol2D;
    [FoldoutGroup("Slam Attack")] public GameObject activeAttackBoxCol2D;
   
    [FoldoutGroup("Slam Attack")] public int maxRandom;
    [FoldoutGroup("Slam Attack")] public int minAttackValue;
    [FoldoutGroup("Slam Attack")] public int randAttackValue;
    [FoldoutGroup("Slam Attack")] public int attackProbabilityBooster;
    #endregion

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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        stateMachine = new StateMachine<KaliBossAI>(this);
        stateMachine.ChangeState(S1IdleState.Instance);
    }

    // Update is called once per frame
    void Update()
    {
        //Debugging, used to test attacking Kali
        if(Input.GetKeyDown(KeyCode.F))
        {
            previousLifepoints = lifePoints;
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

    void WatchForStageChange()
    {
        if(lifePoints <= lifepointsToChangeState)
        {
            bossStage = BossStages.Stage2;
            stateMachine.ChangeState(S2IdleState.Instance);
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