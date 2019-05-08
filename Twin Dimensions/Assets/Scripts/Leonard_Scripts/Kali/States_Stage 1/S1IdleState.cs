using UnityEngine;
using StateData;

public class S1IdleState : State<KaliBossAI>
{
    private static S1IdleState _instance;

    private S1IdleState()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static S1IdleState Instance
    {
        get
        {
            if(_instance == null)
            {
                new S1IdleState();
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
        if(!_owner.attackState)
        {
            _owner.stateMachine.ChangeState(S1AttackState.Instance);
        }
    }
}
