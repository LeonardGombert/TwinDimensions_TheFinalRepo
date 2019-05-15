using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class SlamAttackState : State<KaliBossAI>
{
    private static SlamAttackState _instance;

    private enum SlamAttackDirections {Right, Left}

    KaliBossAI _owner;
    SlamAttackDirections direction;
    Animator anim;

    GameObject activeAttackBoxCol2D;
    GameObject rightAttackBoxCol2D;
    GameObject leftAttackBoxCol2D;
    
    float timePassedSinceLastAttack = 0;
    float timeToReachForAttack = 2f;
    
    KaliBossAI.S1BossStates currentState;
    KaliBossAI.S1BossStates idleState;

    private SlamAttackState()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static SlamAttackState Instance
    {
        get
        {
            if(_instance == null)
            {
                new SlamAttackState();
            }

            return _instance;
        }
    }

    public override void EnterState(KaliBossAI _owner)
    {
        Debug.Log("Entering Attacking State");
        anim = _owner.anim;

        activeAttackBoxCol2D = _owner.activeAttackBoxCol2D;
        rightAttackBoxCol2D = _owner.rightAttackBoxCol2D;
        leftAttackBoxCol2D = _owner.leftAttackBoxCol2D;
        currentState = _owner.Stage1CurrentState;
        
        _owner.isSlamming = true;
        KaliBossAI.isTrackingPlayerSide = false;
        
        //this allow player to "dodge" the attack
        leftAttackBoxCol2D.SetActive(false);
        rightAttackBoxCol2D.SetActive(false);
        activeAttackBoxCol2D.SetActive(true);
    }

    public override void ExitState(KaliBossAI _owner)
    {
        Debug.Log("Exiting Attacking State");
    }

    public override void UpdateState(KaliBossAI _owner)
    {
        Debug.Log("Updating Attack State");
        S1SlamAttack(_owner);       
    }

    void S1SlamAttack(KaliBossAI _owner)
    {
        anim.SetBool("S1SlamAttack", true);

        activeAttackBoxCol2D = _owner.activeAttackBoxCol2D;
        rightAttackBoxCol2D = _owner.rightAttackBoxCol2D;
        leftAttackBoxCol2D = _owner.leftAttackBoxCol2D;

        if(timePassedSinceLastAttack >= timeToReachForAttack)
        {
            activeAttackBoxCol2D.SendMessage("Slamming");
            timePassedSinceLastAttack = 0;
            _owner.Stage1CurrentState = KaliBossAI.S1BossStates.S1Idle;
        }

        else timePassedSinceLastAttack += Time.deltaTime;

        if(_owner.idleState)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);
        }
        
        if(_owner.deathState)
        {
            _owner.stateMachine.ChangeState(DeathState.Instance);
        }
    }
}