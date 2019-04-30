using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class LockPriestController : PriestClass
{
    private string animyes = "Activate";

    // Start is called before the first frame update
    public override void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    public override void Update()
    {
        //LockPlayer();
    }

    private void LockPlayer()
    {
        if (base.MyLayerChecker()) //if on my layer
        {
            //playercanswitch = false;
        }

        if (!base.MyLayerChecker()) //if not on my layer
        {
            //playercanswitch = true;
        }
    }
}