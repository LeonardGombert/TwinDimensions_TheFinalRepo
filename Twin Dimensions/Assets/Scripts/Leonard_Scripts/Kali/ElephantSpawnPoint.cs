﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class ElephantSpawnPoint : MonoBehaviour
{
    [FoldoutGroup("AssignVariable")] [SerializeField] GameObject Elephant;
    [FoldoutGroup("DEBUGVariables")] [SerializeField] bool isOccupied = false;
    [FoldoutGroup("DEBUGVariables")] [ShowInInspector] private bool canSpawnElephant = true;

    float runTime;
    float endTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isOccupied && canSpawnElephant) SpawnElephant();
        else return;
        Timer();
    }

    void Timer()
    {
        if (runTime >= endTime)
        {
            canSpawnElephant = true;
            runTime = 0;
        }

        else runTime += Time.deltaTime;
    }

    void SpawnElephant()
    {
        Instantiate(Elephant, this.transform.position, Quaternion.identity);
        canSpawnElephant = false;
        isOccupied = true;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Elephant")
        {
            isOccupied = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Elephant")
        {
            isOccupied = false;
        }
    }
}
