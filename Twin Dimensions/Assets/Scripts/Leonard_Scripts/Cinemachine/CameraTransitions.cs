using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Editor;

public class CameraTransitions : MonoBehaviour
{

    public static void ChangingWorlds(CinemachineVirtualCamera cam)
    {
        Transform player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cam.m_Follow = player;
    }

    public static void ChangingWorldsBack(CinemachineVirtualCamera cam1, CinemachineVirtualCamera cam2)
    {
        cam1.m_Follow = null;
        cam2.m_Follow = null;
    }

    public static void ChangeCameraOnce(GameObject cam1, GameObject cam2)
    { 
        cam2.SetActive(true);
        cam1.SetActive(false);
    }

    public static void ChangeCameraBack(GameObject cam1, GameObject cam2)
    {
        cam1.SetActive(true);
        cam2.SetActive(false);        
    }
}
