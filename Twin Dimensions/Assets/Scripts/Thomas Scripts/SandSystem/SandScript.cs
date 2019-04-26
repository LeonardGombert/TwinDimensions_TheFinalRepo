using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        SandTextScript.sandAmount += 1;
        Destroy(gameObject);
    }
}
