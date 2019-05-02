using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class SandManager : SerializedMonoBehaviour
{
    public static SandManager instance;

    void Awake()
    {
        if(instance == null)
        {            
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddNewSandShard(int sandGained)
    { 
        int mySandAmount = 0;
        int sandRequieredForKey = 5;
        int myKeys = 0;

        mySandAmount += sandGained;

        Debug.Log("I've currently got " + mySandAmount + " sand shards");

        if(mySandAmount == sandRequieredForKey)
        {
            mySandAmount = 0;
            myKeys += 1;
            Debug.Log("I've currently got " + myKeys + " keys");
        }
    }
}
