using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class S1AttackState : State<KaliBossAI>
{
    /*
        Different Attacks:

        STAGE 1: Slam attack, sweep attack (?)

    */

    private static S1AttackState _instance;

    private enum SlamAttackDirections {Right, Left}

    
    KaliBossAI _owner;
    SlamAttackDirections direction;

    BoxCollider2D activeAttackBoxCol2D;
    BoxCollider2D rightAttackBoxCol2D;
    BoxCollider2D leftAttackBoxCol2D;
    KaliBossAI.BossStates currentState;
    float timeBeforeAttack;
    float runTime;

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
        activeAttackBoxCol2D = _owner.activeAttackBoxCol2D;
        rightAttackBoxCol2D = _owner.rightAttackBoxCol2D;
        leftAttackBoxCol2D = _owner.leftAttackBoxCol2D;

        timeBeforeAttack = _owner.timeBeforeAttack;        
        runTime = _owner.runTime;
        currentState = _owner.currentState;
    }

    public override void ExitState(KaliBossAI _owner)
    {
        Debug.Log("Exiting Attacking State");
    }

    public override void UpdateState(KaliBossAI _owner)
    {
        if(_owner.idleState)
        {
            _owner.stateMachine.ChangeState(S1IdleState.Instance);
        }
        
        if(_owner.deathState)
        {
            _owner.stateMachine.ChangeState(S1DeathState.Instance);
        }
        
        StartCoroutine(SlamAttack());
        //SlamAttack();
    }

    private IEnumerator SlamAttack()
    {
        int attackSide = Random.Range(0, 2);

        if(attackSide == 0) activeAttackBoxCol2D = leftAttackBoxCol2D;
        else if(attackSide == 1) activeAttackBoxCol2D = rightAttackBoxCol2D;

        if (runTime >= timeBeforeAttack)
        {
            activeAttackBoxCol2D.enabled = true;
            Debug.Log("I'm slamming");

            runTime = 0;
            activeAttackBoxCol2D = null;
            currentState = KaliBossAI.BossStates.S1Idle; //switch back to idle after an attack
            yield return null;
        }
        else runTime += Time.fixedUnscaledDeltaTime;
    }

    public void monoParser(MonoBehaviour mono)
    {
        //We can now use StartCoroutine from MonoBehaviour in a non MonoBehaviour script
        mono.StartCoroutine(SlamAttack());

       //And also use StopCoroutine function
        mono.StopCoroutine(SlamAttack());
    }
}
