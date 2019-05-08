using UnityEngine;
using StateData;

public class S1DeathState : State<KaliBossAI>
{
    private static S1DeathState _instance;

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
