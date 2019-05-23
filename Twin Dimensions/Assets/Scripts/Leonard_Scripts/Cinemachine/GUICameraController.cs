using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Cinemachine;
using Cinemachine.Editor;

public class GUICameraController : MonoBehaviour
{
    public static GameObject myGUICamera;
    // Start is called before the first frame update
    void Start()
    {
        myGUICamera = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void MoveCameraToPosition(Vector3 targetPosition, LayerMask layer)
    {
        myGUICamera.SetActive(true);
        myGUICamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
        myGUICamera.layer = layer;
    }
}
