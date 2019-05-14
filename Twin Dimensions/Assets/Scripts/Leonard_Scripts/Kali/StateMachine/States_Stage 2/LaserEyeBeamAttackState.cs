using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateData;

public class LaserEyeBeamAttackState : State<KaliBossAI>
{
    #region //STATE MACHINE VARIABLES
    static LaserEyeBeamAttackState _instance;
    KaliBossAI _owner;
    KaliBossAI.S2BossStates currentState;
    KaliBossAI.S2BossStates idleState;
    #endregion

    #region //LASER EYE BEAM
    LineRenderer laserBeam;
    Transform laserSpawn;
    Transform laserTarget;
    List<Transform> laserPointList = new List<Transform>();
    #endregion

    #region //GENERAL VARIABLES
    float timePassedSinceLastAttack = 0;
    float timeToReachForAttack = 2f;
    Animator anim;
    #endregion

    private LaserEyeBeamAttackState()
    {
        if(_instance != null) _instance = this;
    }

    public static LaserEyeBeamAttackState Instance
    {
        get
        {
            if(_instance == null) new LaserEyeBeamAttackState();
            return _instance;
        }
    }

    public override void EnterState(KaliBossAI _owner)
    {
        Debug.Log("Entering Attacking State");

        //LASER EYE BEAM VARIABLES
        laserBeam = _owner.laserBeam;
        laserSpawn = _owner.laserSpawn;
        laserTarget = _owner.laserTarget;
        laserPointList = _owner.laserPointList;
    }

    public override void ExitState(KaliBossAI _owner)
    {
        Debug.Log("Exiting Attacking State");
    }

    public override void UpdateState(KaliBossAI _owner)
    {
        Debug.Log("Updating Attack State");        
        
        anim.SetBool("S2SlamAttack", true);

        S2SlamAttack(_owner);           
    }

    void S2SlamAttack(KaliBossAI _owner)
    {

    }
}
