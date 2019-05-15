/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateData;

public class GeneralAttackStateManager : State<KaliBossAI>
{
    private static GeneralAttackStateManager _instance;

    private GeneralAttackStateManager()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static GeneralAttackStateManager Instance
    {
        get
        {
            if(_instance == null)
            {
                new GeneralAttackStateManager();
            }

            return _instance;
        }
    }

    public override void EnterState(KaliBossAI _owner)
    {
        Debug.Log("Entering Idle State");
    }

    public override void ExitState(KaliBossAI _owner)
    {
        Debug.Log("Exiting Idle State");
    }

    public override void UpdateState(KaliBossAI _owner)
    {
        if(_owner.attackState)
        {
            _owner.stateMachine.ChangeState(SlamAttackState.Instance);
        }

        if(_owner.idleState)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);
        }
    }
}
*/