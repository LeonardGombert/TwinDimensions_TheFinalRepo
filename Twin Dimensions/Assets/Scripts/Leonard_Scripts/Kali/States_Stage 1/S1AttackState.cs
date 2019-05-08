using UnityEngine;
using StateData;

public class S1AttackState : State<KaliBossAI>
{
    private static S1AttackState _instance;

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
    }

    public override void ExitState(KaliBossAI _owner)
    {
        Debug.Log("Exiting Attacking State");
    }

    public override void UpdateState(KaliBossAI _owner)
    {
        if(_owner.attackState)
        {
            _owner.stateMachine.ChangeState(S1IdleState.Instance);
        }
    }
}
