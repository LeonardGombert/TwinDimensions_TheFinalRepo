using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorOpener : MonoBehaviour
{
    public enum animNamesList // your custom enumeration
    {
        Kali_Bleu, 
        Kali_Jaune, 
        Kali_Orange,
        Kali_Pink,
        Kali_Purple,
        Kali_Rouge,
        Kali_Turquoise,
        Kali_Vert,
        Real_Bleu, 
        Real_Jaune, 
        Real_Orange,
        Real_Pink,
        Real_Purple,
        Real_Rouge,
        Real_Turquoise,
        Real_Vert,
    };

    public enum spriteNamesList // your custom enumeration
    {
        Kali_Bleu, 
        Kali_Jaune, 
        Kali_Orange,
        Kali_Pink,
        Kali_Purple,
        Kali_Rouge,
        Kali_Turquoise,
        Kali_Vert,
        Real_Bleu, 
        Real_Jaune, 
        Real_Orange,
        Real_Pink,
        Real_Purple,
        Real_Rouge,
        Real_Turquoise,
        Real_Vert,
    };

    public animNamesList animDropdown;
    
    //public spriteNamesList spriteDropdown;

    [SerializeField]
    List<Sprite> spriteList = new List<Sprite>();
    
    string animName;

    Animator anim;
    BoxCollider2D boxcol2D;
    SpriteRenderer sr;

    void Awake()
    {
        anim = GetComponent<Animator>();
        boxcol2D = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        animName = animDropdown.ToString();
        Debug.Log("I've selected " + animName);
    }

    void Activated()
    {
        if(boxcol2D.enabled)
        {
            anim.SetFloat ("Direction", 1);
            anim.Play(animName);
            boxcol2D.enabled = false;            
        }

        else
        {
            anim.SetFloat ("Direction", -1);
			anim.Play(animName, -1, float.NegativeInfinity);
            boxcol2D.enabled = true;
        }
    }

    void Released()
    {
        if(boxcol2D.enabled)
        {   
            anim.SetFloat ("Direction", 1);
            anim.Play(animName);
            boxcol2D.enabled = false;
        }

        else
        {
            anim.SetFloat ("Direction", -1);
			anim.Play (animName, -1, float.NegativeInfinity);
            boxcol2D.enabled = true;
        }
    }
}
