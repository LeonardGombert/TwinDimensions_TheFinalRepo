using UnityEngine;
using StateData;

public class SweepAttackState : State<KaliBossAI>
{
    private static SweepAttackState _instance;

    private SweepAttackState()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static SweepAttackState Instance
    {
        get
        {
            if(_instance == null)
            {
                new SweepAttackState();
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
