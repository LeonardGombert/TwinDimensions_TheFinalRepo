using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class DisplayKeys : MonoBehaviour
{
    public enum AnimatorBool{isE, isAlt, isSpace,}
    
    [SerializeField] GameObject printKey;    
    [SerializeField] AnimatorBool animBool;

    Animator anim;

    string boolString;

    private void Start ()
    {
        printKey.gameObject.SetActive(false);
        anim = printKey.GetComponent<Animator>();
    }

    void UpdateMode()
    {
        //boolString = AnimatorBool
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Activating");
            printKey.gameObject.SetActive(true);
            anim.SetBool(animBool.ToString(), true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            printKey.gameObject.SetActive(false);
            anim.SetBool(animBool.ToString(), false);
        }
    }
}
