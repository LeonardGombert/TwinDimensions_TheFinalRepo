﻿using UnityEngine;
using StateData;

public class S1DeathState : State<KaliBossAI>
{
    private static S1DeathState _instance;

    BoxCollider2D activeAttackBoxCol2D;
    BoxCollider2D rightAttackBoxCol2D;
    BoxCollider2D leftAttackBoxCol2D;

    private S1DeathState()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static S1DeathState Instance
    {
        get
        {
            if(_instance == null)
            {
                new S1DeathState();
            }

            return _instance;
        }
    }

    public override void EnterState(KaliBossAI _owner)
    {
        Debug.Log("Entering Idle State");
        activeAttackBoxCol2D = _owner.activeAttackBoxCol2D;
        rightAttackBoxCol2D = _owner.rightAttackBoxCol2D;
        leftAttackBoxCol2D = _owner.leftAttackBoxCol2D;
        
        rightAttackBoxCol2D.enabled = false;
        leftAttackBoxCol2D.enabled = false;
    }

    public override void ExitState(KaliBossAI _owner)
    {
        Debug.Log("Exiting Idle State");
    }

    public override void UpdateState(KaliBossAI _owner)
    {
        if(_owner.attackState)
        {
            _owner.stateMachine.ChangeState(S1AttackState.Instance);
        }

        if(_owner.idleState)
        {
            _owner.stateMachine.ChangeState(S1IdleState.Instance);
        }
    }
}
