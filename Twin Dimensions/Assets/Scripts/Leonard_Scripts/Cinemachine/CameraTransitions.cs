using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitions : MonoBehaviour
{
    [SerializeField] GameObject cam1 = default;
    [SerializeField] GameObject cam2 = default;

    public void ChangeCamera()
    {
        cam2.SetActive(true);
        cam1.SetActive(false);
    }
}
