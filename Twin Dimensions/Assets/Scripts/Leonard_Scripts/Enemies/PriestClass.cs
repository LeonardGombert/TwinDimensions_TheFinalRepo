﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class PriestClass : MonoBehaviour
{
    public static bool isOnMyLayer = false;

    [FoldoutGroup("References")][SerializeField]
    public GameObject player;
    
    public SpriteRenderer sr;
    public Animator anim;

    public bool activatedByTurret = false;

    [FoldoutGroup("Sand")][SerializeField] 
    GameObject sandToDrop;
    
    int amountOfSandToDrop;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
    
    void IsBeingActivatedByTurret()
    {
        
    }

    public bool MyLayerChecker(bool isOnSameLayer)
    {
        if(isOnSameLayer == true)
        {
            Debug.Log(gameObject.name + " is Thicco Mode"); 
            return true;
        }
        
        if(isOnSameLayer == false) 
        {
            Debug.Log(gameObject.name + " ain't Thicco Mode"); 
            return false;
        }
        
        else return false;
    }

    void DropSand(int sandAmount)
    {
        amountOfSandToDrop = sandAmount;
    }

    void GenerateSand(int amountOfSandToDrop)
    {
        for (int i = 0; i < amountOfSandToDrop; ++i)
        {
            Instantiate(sandToDrop, transform.position, Quaternion.identity);
        }
    }
}