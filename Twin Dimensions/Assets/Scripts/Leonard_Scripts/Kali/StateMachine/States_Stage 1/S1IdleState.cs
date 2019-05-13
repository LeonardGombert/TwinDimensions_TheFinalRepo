using UnityEngine;
using StateData;

public class S1IdleState : State<KaliBossAI>
{
    private static S1IdleState _instance;

    GameObject activeAttackBoxCol2D;
    GameObject rightAttackBoxCol2D;
    GameObject leftAttackBoxCol2D;
    GameObject yeet;
    
    float timePassedSinceLastAttack = 0;
    float timeToReachForAttack = 5;

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
    }

    public override void ExitState(KaliBossAI _owner)
    {
        Debug.Log("Exiting Idle State");
    }

    public override void UpdateState(KaliBossAI _owner)
    {
        Debug.Log("Updating Idle State");

        if(timePassedSinceLastAttack >= timeToReachForAttack)
        {
            activeAttackBoxCol2D = yeet;
            
            Debug.Log("yeet");
            yeet.SendMessage("Slamming");
            timePassedSinceLastAttack = 0;
        }

        else timePassedSinceLastAttack += Time.deltaTime;

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
