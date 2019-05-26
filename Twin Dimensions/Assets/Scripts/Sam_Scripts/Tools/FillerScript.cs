using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillerScript : MonoBehaviour
{
    BoxCollider2D bCollider;

    bool fillerPosA;
    bool fillerPosB;

    void Awake()
    {
        bCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (GetComponentInParent<PlatformScript>().posA == true)
        {
            bCollider.enabled = false;
        }

        else if (GetComponentInParent<PlatformScript>().posA == false)
        {
            bCollider.enabled = true;
        }
    }
}
