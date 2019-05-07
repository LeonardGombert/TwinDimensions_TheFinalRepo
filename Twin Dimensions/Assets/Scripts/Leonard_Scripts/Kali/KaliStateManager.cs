using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaliStateManager : MonoBehaviour
{
    BossStates defautState = BossStates.Idle;
    BossStates currentState;

    Animator anim;

    public enum BossStates
    {
        Idle,
        Dead,
        Laughing,
        Attacking,
        Transitioning        
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) currentState = defautState;
        if(Input.GetKeyDown(KeyCode.A)) currentState = BossStates.Dead;
        if(Input.GetKeyDown(KeyCode.Z)) currentState = BossStates.Laughing;
        if(Input.GetKeyDown(KeyCode.E)) currentState = BossStates.Attacking;
        if(Input.GetKeyDown(KeyCode.R)) currentState = BossStates.Transitioning;
        CheckCurrentState();
    }

    void CheckCurrentState()
    {
        switch(currentState)
        {
            case BossStates.Idle : 
            Debug.Log("Am Idle");
            anim.SetFloat("BlendX", 0);
            anim.SetFloat("BlendY", 0); 
            break;

            case BossStates.Dead : 
            Debug.Log("Am Dead");
            anim.SetFloat("BlendX", 0);
            anim.SetFloat("BlendY", 1); 
            break;

            case BossStates.Laughing : 
            Debug.Log("Am Laughing"); 
            anim.SetTrigger("isLaughing"); 
            break;

            case BossStates.Attacking : 
            Debug.Log("Am Attacking");
            anim.SetTrigger("isAttacking"); 
            break;

            case BossStates.Transitioning : 
            Debug.Log("Am Transitioning"); 
            anim.SetTrigger("isTransition"); 
            break;

            default :
            Debug.Log("Am Idle");
            anim.SetTrigger("isIdle"); 
            break;
        }
    }
}
