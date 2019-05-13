using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class S1AttackState : State<KaliBossAI>
{
    private static S1AttackState _instance;

    private enum SlamAttackDirections {Right, Left}

    KaliBossAI _owner;
    SlamAttackDirections direction;
    Animator anim;

    BoxCollider2D activeAttackBoxCol2D;
    BoxCollider2D rightAttackBoxCol2D;
    BoxCollider2D leftAttackBoxCol2D;
    KaliBossAI.BossStates currentState;
    KaliBossAI.BossStates idleState;

    private S1AttackState()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static S1AttackState Instance
    {
        get
        {
            if(_instance == null)
            {
                new S1AttackState();
            }

            return _instance;
        }
    }

    public override void EnterState(KaliBossAI _owner)
    {
        Debug.Log("Entering Attacking State");
        activeAttackBoxCol2D = _owner.activeAttackBoxCol2D;
        rightAttackBoxCol2D = _owner.rightAttackBoxCol2D;
        leftAttackBoxCol2D = _owner.leftAttackBoxCol2D;
        currentState = _owner.currentState;

        _owner.anim = anim;
    }

    public override void ExitState(KaliBossAI _owner)
    {
        Debug.Log("Exiting Attacking State");
    }

    public override void UpdateState(KaliBossAI _owner)
    {
        Debug.Log("Updating Attack State");

        SlamAttack();

        if(_owner.idleState)
        {
            _owner.stateMachine.ChangeState(S1IdleState.Instance);
        }
        
        if(_owner.deathState)
        {
            _owner.stateMachine.ChangeState(S1DeathState.Instance);
        }        
    }

    private void SlamAttack()
    {
        
    }
}