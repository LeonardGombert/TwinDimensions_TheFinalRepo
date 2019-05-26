using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendreParchemin : MonoBehaviour
{
    public Animator anim;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            anim.SetBool("isOpen", true);
        }
    }
}
