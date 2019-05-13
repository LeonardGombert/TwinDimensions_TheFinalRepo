using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsManager : MonoBehaviour
{
    public static bool isMoving = false;
    public static bool isGuarding = false;
    public static bool isPunching = false;
    public static bool isInvoking = false;
    public static bool isDead = false;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        CheckCurrentState();
    }

    void CheckCurrentState()
    {
        if(isMoving == true)
        {
            isGuarding = false;
            isPunching = false;
            isInvoking = false;
            isDead = false;

            anim.SetFloat("animTypeX", 0);
            anim.SetFloat("animTypeY", 0);
        }

        if(isGuarding == true)
        {
            isMoving = false;
            isPunching = false;
            isInvoking = false;
            isDead = false;

            anim.SetFloat("animTypeX", 0);
            anim.SetFloat("animTypeY", 1);
        }

        if(isPunching == true)
        {
            isMoving = false;
            isGuarding = false;
            isInvoking = false;
            isDead = false;

            anim.SetFloat("animTypeX", 1);
            anim.SetFloat("animTypeY", 0);
        }

        if(isInvoking == true)
        {
            isMoving = false;
            isGuarding = false;
            isPunching = false;
            isDead = false;

            anim.SetFloat("animTypeX", 0);
            anim.SetFloat("animTypeY", -1);
        }

        if(isDead == true)
        {
            isMoving = false;
            isGuarding = false;
            isPunching = false;
            isInvoking = false;

            anim.SetFloat("animTypeX", -1);
            anim.SetFloat("animTypeY", 0);
        }

        else 
        {
            anim.SetFloat("animTypeX", 0);
            anim.SetFloat("animTypeY", 0);
            anim.SetFloat("xDirection", 0);
            anim.SetFloat("yDirection", 0);
        }
    }
}
