using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class SandManager : SerializedMonoBehaviour
{
    List<List<GameObject>> BigList = new List<List<GameObject>>();
    [FoldoutGroup("SandHolder Lists")][SerializeField]
    List<GameObject> sandHolders = new List<GameObject>();
    [FoldoutGroup("SandHolder Lists")]
    List<GameObject> mediumSand = new List<GameObject>();
    [FoldoutGroup("SandHolder Lists")]
    List<GameObject> highSand = new List<GameObject>();
    [FoldoutGroup("SandHolder Lists")]
    List<GameObject> extremeSand = new List<GameObject>();

    [FoldoutGroup("Sand Amounts")]
    int littleAmount = 1;
    [FoldoutGroup("Sand Amounts")]
    int mediumAmount;
    [FoldoutGroup("Sand Amounts")]
    int highAmount;
    [FoldoutGroup("Sand Amounts")]
    int extremeAmount;

    int sandAmount;

    int mySandAmount = 0;
    int sandRequiredForKey = 5;
    int myKeys = 0;

    void Start()
    {
        BigList.AddRange(new List<GameObject>[] { sandHolders, mediumSand, highSand, extremeSand });
    }

    // Update is called once per frame
    void Update()
    {
       SandHeldByEntity();
    }

    void AddNewSandShard(int sandGained)
    {
        mySandAmount = mySandAmount + sandGained;

        Debug.Log("I've currently got " + mySandAmount + " sand shards");

        if(mySandAmount == sandRequiredForKey)
        {
            mySandAmount = 0;
            myKeys += 1;
            Debug.Log("I've currently got " + myKeys + " keys");
        }
    }

    private void SandHeldByEntity()
    {
        foreach (List<GameObject> list in BigList)
        {
            if(list == sandHolders) sandAmount = littleAmount;
            if(list == mediumSand) sandAmount = mediumAmount; 
            if(list == highSand) sandAmount = highAmount;
            if(list == extremeSand) sandAmount = extremeAmount;
            
            foreach (GameObject Entity in list)
            {
                Entity.SendMessage("DropSand", sandAmount);
            }
        }
    }
}
