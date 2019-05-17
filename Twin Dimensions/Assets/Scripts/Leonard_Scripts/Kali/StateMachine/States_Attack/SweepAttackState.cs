/*using UnityEngine;
using StateData;

public class SweepAttackState : State<KaliBossAI>
{
    private static SweepAttackState _instance;

    KaliBossAI _owner;
    
    Animator anim;

    GameObject activeAttackBoxCol2D;
    GameObject rightAttackBoxCol2D;
    GameObject leftAttackBoxCol2D;
    
    KaliBossAI.S2BossStates currentState;
    KaliBossAI.S2BossStates idleState;

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

    void S2SweepAttack (KaliBossAI _owner)
    {
        anim.SetBool("S1SlamAttack", true);

        activeAttackBoxCol2D = _owner.activeAttackBoxCol2D;
        rightAttackBoxCol2D = _owner.rightAttackBoxCol2D;
        leftAttackBoxCol2D = _owner.leftAttackBoxCol2D;

        if(_owner.idleState)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);
        }
        
        if(_owner.deathState)
        {
            _owner.stateMachine.ChangeState(DeathState.Instance);
        }




        // Move our position a step closer to the target.
        float step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            // Swap the position of the cylinder.
            target.position *= -1.0f;
        }
    }
}
*/