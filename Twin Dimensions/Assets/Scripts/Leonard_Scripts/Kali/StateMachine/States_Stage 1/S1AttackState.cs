﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class S1AttackState : State<KaliBossAI>
{
    private static S1AttackState _instance;

    private enum SlamAttackDirections {Right, Left}

    KaliBossAI _owner;
    SlamAttackDirections direction;
    Animator anim;

    GameObject activeAttackBoxCol2D;
    GameObject rightAttackBoxCol2D;
    GameObject leftAttackBoxCol2D;
    
    float timePassedSinceLastAttack = 0;
    float timeToReachForAttack = 2f;
    
    KaliBossAI.BossStates currentState;
    KaliBossAI.BossStates idleState;

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
        currentState = _owner.currentState;
        anim = _owner.anim;

        _owner.isSlamming = true;

        KaliBossAI.isTrackingPlayerSide = false;
        
        //this allow player to "dodge" the attack
        leftAttackBoxCol2D.SetActive(false);
        rightAttackBoxCol2D.SetActive(false);
        activeAttackBoxCol2D.SetActive(true);
    }

    public override void ExitState(KaliBossAI _owner)
    {
        Debug.Log("Exiting Attacking State");
    }

    public override void UpdateState(KaliBossAI _owner)
    {
        Debug.Log("Updating Attack State");        
        
        anim.SetBool("S1SlamAttack", true);

        activeAttackBoxCol2D = _owner.activeAttackBoxCol2D;
        rightAttackBoxCol2D = _owner.rightAttackBoxCol2D;
        leftAttackBoxCol2D = _owner.leftAttackBoxCol2D;


        if(timePassedSinceLastAttack >= timeToReachForAttack)
        {
            activeAttackBoxCol2D.SendMessage("Slamming");
            timePassedSinceLastAttack = 0;
            _owner.currentState = KaliBossAI.BossStates.S1Idle;
        }

        else timePassedSinceLastAttack += Time.deltaTime;

        if(_owner.idleState)
        {
            _owner.stateMachine.ChangeState(S1IdleState.Instance);
        }
        
        if(_owner.deathState)
        {
            _owner.stateMachine.ChangeState(S1DeathState.Instance);
        }        
    }
}