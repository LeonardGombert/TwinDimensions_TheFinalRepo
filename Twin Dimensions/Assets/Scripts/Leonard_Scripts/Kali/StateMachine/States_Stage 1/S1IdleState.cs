using UnityEngine;
using StateData;

public class S1IdleState : State<KaliBossAI>
{
    private static S1IdleState _instance;
                   
    GameObject activeAttackBoxCol2D;
    GameObject rightAttackBoxCol2D;
    GameObject leftAttackBoxCol2D;

    Animator anim;

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
        activeAttackBoxCol2D = _owner.activeAttackBoxCol2D;
        rightAttackBoxCol2D = _owner.rightAttackBoxCol2D;
        leftAttackBoxCol2D = _owner.leftAttackBoxCol2D;
        anim = _owner.anim;

        KaliBossAI.isTrackingPlayerSide = true;

        leftAttackBoxCol2D.SetActive(true);
        rightAttackBoxCol2D.SetActive(true);
    }

    public override void ExitState(KaliBossAI _owner)
    {
        Debug.Log("Exiting Idle State");
    }

    public override void UpdateState(KaliBossAI _owner)
    {
        Debug.Log("Updating Idle State");
        
        anim.SetBool("S1SlamAttack", false);
        anim.SetBool("S1Idle", true);

        if(_owner.attackState)
        {
            _owner.stateMachine.ChangeState(S1AttackState.Instance);
        }

        if(_owner.deathState)
        {
            _owner.stateMachine.ChangeState(S1DeathState.Instance);
        }
    }
}
