using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintKey : MonoBehaviour
{
    Animator anim;
    public enum AnimatorKeys{isInteracting, isALt, isSpace}

    public AnimatorKeys animKeys;

    private void Start ()
    {
        anim = GameObject.FindGameObjectWithTag("ButtonMap").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            anim.SetBool(animKeys.ToString(), true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            anim.SetBool(animKeys.ToString(), false);
        }
    }
}
