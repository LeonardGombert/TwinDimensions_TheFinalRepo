
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    Vector2 positionA;
    Vector2 positionB;

    public Transform objectPositionA;
    public Transform objectPositionB;
    
    public bool posA = true;
    public bool posB = false;

    BoxCollider2D bCollider;

    void Awake()
    {
        bCollider = gameObject.GetComponent<BoxCollider2D>();

        positionA = objectPositionA.position;
        positionB = objectPositionB.position;

        gameObject.transform.position = positionA;
    }

    void Activated()
    {
        if(posA == true)
        {
            gameObject.transform.position = positionB;
            posA = false;
            posB = true;

            bCollider.enabled = false;
        }
        else
        {
            gameObject.transform.position = positionA;
            posA = true;
            posB = false;

            bCollider.enabled = true;
        }
    }

    void Released()
    {
        if(posA == true)
        {
            gameObject.transform.position = positionB;
            posA = false;
            posB = true;

            bCollider.enabled = false;
        }
        else
        {
            gameObject.transform.position = positionA;
            posA = true;
            posB = false;

            bCollider.enabled = true;
        }
    }
}
