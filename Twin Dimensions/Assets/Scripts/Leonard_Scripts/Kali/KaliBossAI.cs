using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateData;

public class KaliBossAI : MonoBehaviour
{
    BossStates defautState = BossStates.Idle;
    BossStates currentState;
    
    Animator anim;
    
    public StateMachine<KaliBossAI> stateMachine { get; set; }
    public bool attackState = false;
    public float gameTimer;
    public int seconds = 0;

    public enum BossStates
    {
        Idle,
        Dead,
        Laughing,
        Attacking,
        Transitioning        
    }
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();

        stateMachine = new StateMachine<KaliBossAI>(this);
        stateMachine.ChangeState(S1IdleState.Instance);
        gameTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) currentState = defautState;
        if(Input.GetKeyDown(KeyCode.A)) currentState = BossStates.Dead;
        if(Input.GetKeyDown(KeyCode.Z)) currentState = BossStates.Laughing;
        if(Input.GetKeyDown(KeyCode.E)) currentState = BossStates.Attacking;
        if(Input.GetKeyDown(KeyCode.R)) currentState = BossStates.Transitioning;
        //CheckCurrentState();

        if(Time.time > gameTimer + 1)
        {
            gameTimer = Time.time;
            seconds++;
            Debug.Log(seconds);
        }

        if(seconds == 5)
        {
            seconds = 0;
            attackState = !attackState;
        }

        stateMachine.Update();
    }

    void CheckCurrentState()
    {
        switch(currentState)
        {
            case BossStates.Idle : 
            Debug.Log("Am Idle");
            anim.SetFloat("BlendX", 0);
            anim.SetFloat("BlendY", 0); 
            break;

            case BossStates.Dead : 
            Debug.Log("Am Dead");
            anim.SetFloat("BlendX", 0);
            anim.SetFloat("BlendY", 1); 
            break;

            case BossStates.Laughing : 
            Debug.Log("Am Laughing"); 
            anim.SetTrigger("isLaughing"); 
            break;

            case BossStates.Attacking : 
            Debug.Log("Am Attacking");
            anim.SetTrigger("isAttacking"); 
            break;

            case BossStates.Transitioning : 
            Debug.Log("Am Transitioning"); 
            anim.SetTrigger("isTransition"); 
            break;

            default :
            Debug.Log("Am Idle");
            anim.SetTrigger("isIdle"); 
            break;
        }
    }
}
