﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Cinemachine;

public class GUICameraController : MonoBehaviour
{
    [FoldoutGroup("MyCameras")] public static GameObject myGUICamera1;
    [FoldoutGroup("MyCameras")] public static GameObject myGUICamera2;

    // Start is called before the first frame update
    void Start()
    {
        myGUICamera1 = GameObject.FindGameObjectWithTag("GUI World1");
        myGUICamera2 = GameObject.FindGameObjectWithTag("GUI World2");

        myGUICamera1.SetActive(false);
        myGUICamera2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void MoveCameraToPosition(GameObject targetPosition, LayerMask layer)
    {
        if(LayerManager.ObjectIsInRealWorld(targetPosition))
        {
            myGUICamera1.SetActive(true);
            myGUICamera1.transform.position = new Vector3(targetPosition.transform.position.x, targetPosition.transform.position.y, -10);
            myGUICamera1.layer = layer;
        }

        if(!LayerManager.ObjectIsInRealWorld(targetPosition))
        {
            myGUICamera2.SetActive(true);
            myGUICamera2.transform.position = new Vector3(targetPosition.transform.position.x, targetPosition.transform.position.y, -10);
            myGUICamera2.layer = layer;
        }        
    }

    public static void ClearCameraPosition()
    {   
        myGUICamera1.SetActive(false);
        myGUICamera2.SetActive(false);
    }
}
