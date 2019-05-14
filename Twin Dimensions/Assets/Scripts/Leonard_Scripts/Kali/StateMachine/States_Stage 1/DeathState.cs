using UnityEngine;
using StateData;

public class DeathState : State<KaliBossAI>
{
    private static DeathState _instance;

    GameObject activeAttackBoxCol2D;
    GameObject rightAttackBoxCol2D;
    GameObject leftAttackBoxCol2D;

    private DeathState()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static DeathState Instance
    {
        get
        {
            if(_instance == null)
            {
                new DeathState();
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
            _owner.stateMachine.ChangeState(SlamAttackState.Instance);
        }

        if(_owner.idleState)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);
        }
    }
}
