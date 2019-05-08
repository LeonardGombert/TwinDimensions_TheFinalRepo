using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateData;

public class KaliBossAI : MonoBehaviour
{
    public BossStates defautState = BossStates.S1Idle;
    public BossStates currentState;
    
    Animator anim;
    
    public StateMachine<KaliBossAI> stateMachine { get; set; }
    public bool attackState = false;
    public bool idleState = false;
    public bool deathState = false;

    //public float gameTimer;
    //public int seconds = 0;

    public enum BossStates
    {
        S1Idle,
        S1Dead,
        S1Attacking,

        S2Idle,
        S2Dead,
        S2Attacking,  
    }

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();

        stateMachine = new StateMachine<KaliBossAI>(this);
        stateMachine.ChangeState(S1IdleState.Instance);
        //gameTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) currentState = BossStates.S1Idle;
        if(Input.GetKeyDown(KeyCode.A)) currentState = BossStates.S1Dead;
        if(Input.GetKeyDown(KeyCode.E)) currentState = BossStates.S1Attacking;
        //CheckCurrentState();

        stateMachine.Update();
    }

    void CheckCurrentState()
    {
        switch(currentState)
        {
            case BossStates.S1Idle : 
            Debug.Log("Am Idle");
            break;

            case BossStates.S1Dead : 
            Debug.Log("Am Dead"); 
            break;

            case BossStates.S1Attacking : 
            Debug.Log("Am Attacking"); 
            break;

            case BossStates.S2Idle : 
            Debug.Log("Am Idle");
            break;

            case BossStates.S2Dead : 
            Debug.Log("Am Dead"); 
            break;

            case BossStates.S2Attacking : 
            Debug.Log("Am Attacking"); 
            break;

            default :
            Debug.Log("Am Idle");
            break;
        }
    }
}
