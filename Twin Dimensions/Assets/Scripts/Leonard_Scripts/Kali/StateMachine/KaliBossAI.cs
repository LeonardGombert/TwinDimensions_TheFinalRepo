﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateData;

public class KaliBossAI : MonoBehaviour
{
    public BossStates defautState = BossStates.S1Idle;
    public BossStates currentState;
    
    public Animator anim;
    
    public StateMachine<KaliBossAI> stateMachine { get; set; }
    public bool attackState = false;
    public bool idleState = false;
    public bool deathState = false;

    [SerializeField] float lifePoints = 1000;
    [SerializeField] float damageValue = 10;
    [SerializeField] float previousLifepoints;
    
    #region //SLAM ATTACK
    [FoldoutGroup("Slam Attack")] public BoxCollider2D rightAttackBoxCol2D;
    [FoldoutGroup("Slam Attack")] public BoxCollider2D leftAttackBoxCol2D;
    [FoldoutGroup("Slam Attack")] public BoxCollider2D activeAttackBoxCol2D;

    [FoldoutGroup("Slam Attack")] public int maxRandom;
    [FoldoutGroup("Slam Attack")] public int minAttackValue;
    [FoldoutGroup("Slam Attack")] public int randAttackValue;
    [FoldoutGroup("Slam Attack")] public int attackProbabilityBooster;
    #endregion

    public enum BossStates
    {
        S1Idle,
        S1Dead,
        S1Attacking,

        S2Idle,
        S2Dead,
        S2Attacking,  
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        stateMachine = new StateMachine<KaliBossAI>(this);
        stateMachine.ChangeState(S1IdleState.Instance);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            previousLifepoints = lifePoints;
            lifePoints -= damageValue;
            StartCoroutine(StateSwitch());
        }

        CheckCurrentState();

        stateMachine.Update();
    }

    IEnumerator StateSwitch()
    {
        randAttackValue = Random.Range(0, maxRandom);

        Debug.Log(randAttackValue);

        if(randAttackValue >= minAttackValue) minAttackValue += attackProbabilityBooster; //does not attack, raise probability nothing happens
        else if(randAttackValue <= minAttackValue) currentState = BossStates.S1Attacking; //attacks, repeat

        yield return null;
    }

    public void StartChildCoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }

    private void WhichSide(GameObject side)
    {
        Debug.Log("Player is on " + side.gameObject.name);
    }
    
    void CheckCurrentState()
    {
        switch(currentState)
        {
            case BossStates.S1Idle :
            attackState = false;
            idleState = true;
            deathState = false;
            break;
            
            case BossStates.S1Dead :
            attackState = false;
            idleState = false;
            deathState = true;
            break;

            case BossStates.S1Attacking :
            attackState = true;
            idleState = false;
            deathState = false;
            break;

            case BossStates.S2Idle :
            break;

            case BossStates.S2Dead :
            break;

            case BossStates.S2Attacking :
            break;

            default :
            Debug.Log("null");
            break;
        }
    }
}