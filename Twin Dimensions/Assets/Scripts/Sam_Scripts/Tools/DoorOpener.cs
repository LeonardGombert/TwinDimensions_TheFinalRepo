using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    enum animNamesList // your custom enumeration
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

    enum spriteNamesList // your custom enumeration
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

    [SerializeField]
    animNamesList dropdown;
    [SerializeField]
    animNamesList dropdown2;

    [SerializeField]
    public List<Sprite> spriteList = new List<Sprite>();
    
    string animName;
    string animName2;

    [SerializeField]
    Animator anim;
    BoxCollider2D boxcol2D;
    SpriteRenderer sr;

    void Awake()
    {
        boxcol2D = gameObject.GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        if(boxcol2D.enabled)
        {
            sr.sprite = spriteList[1];
        }
        else
        {
            sr.sprite = spriteList[0];
        }

        animName = dropdown.ToString();
        animName2 = dropdown2.ToString();
        Debug.Log("I've selected " + animName);
    }

    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            anim.Play(animName);
            anim.Update(0f);
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            anim.SetFloat ("Direction", -1);
			anim.Play (animName, -1, float.NegativeInfinity);
            anim.Update(0f);
        }
        */
    }

    void Activated()
    {
        if(boxcol2D.enabled)
        {   
            //anim.Play(animName);
            sr.sprite = spriteList[0];
            boxcol2D.enabled = false;            
        }

        else
        {
            /*anim.SetFloat ("Direction", -1);
			anim.Play(animName, -1, float.NegativeInfinity);*/

            sr.sprite = spriteList[1];
            boxcol2D.enabled = true;
        }
    }

    void Released()
    {
        if(boxcol2D.enabled)
        {      
            //anim.Play(animName);
            sr.sprite = spriteList[0];
            boxcol2D.enabled = false;
        }

        else
        {
            /*anim.SetFloat ("Direction", -1);
			anim.Play (animName, -1, float.NegativeInfinity);*/

            sr.sprite = spriteList[1];
            boxcol2D.enabled = true;
        }
    }
}
