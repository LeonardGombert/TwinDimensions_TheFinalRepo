using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoseSand : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        SandTextScript.sandAmount -= 2;
    }
}
