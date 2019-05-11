using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoScript : MonoBehaviour
{
    void Start()
    {
        StateData nonMonoScript = new StateData();
        //Pass MonoBehaviour to non MonoBehaviour class
        nonMonoScript.monoParser(this);
    }
}
