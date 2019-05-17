/*using UnityEngine;
using StateData;

public class IdleState : State<KaliBossAI>
{
    private static IdleState _instance;
                   
    GameObject activeAttackBoxCol2D;
    GameObject rightAttackBoxCol2D;
    GameObject leftAttackBoxCol2D;

    Animator anim;

    private IdleState()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if(_instance == null)
            {
                new IdleState();
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
            _owner.stateMachine.ChangeState(SlamAttackState.Instance);
        }

        if(_owner.deathState)
        {
            _owner.stateMachine.ChangeState(DeathState.Instance);
        }
    }
}
*/