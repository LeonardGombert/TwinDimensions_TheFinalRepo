using UnityEngine;
using StateData;

public class S2DeathState : State<KaliBossAI>
{
    private static S2DeathState _instance;

    GameObject activeAttackBoxCol2D;
    GameObject rightAttackBoxCol2D;
    GameObject leftAttackBoxCol2D;

    private S2DeathState()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static S2DeathState Instance
    {
        get
        {
            if(_instance == null)
            {
                new S2DeathState();
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
        
        rightAttackBoxCol2D.SetActive(false);
        leftAttackBoxCol2D.SetActive(false);
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
