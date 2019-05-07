using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    Vector2 positionA;
    Vector2 positionB;

    public Transform objectPositionA;
    public Transform objectPositionB;

    void Awake()
    {
        positionA = objectPositionA.position;
        positionB = objectPositionB.position;

        gameObject.transform.position = positionA;
    }

    void Activated()
    {
        gameObject.transform.position = positionB;
    }

    void Released()
    {
        gameObject.transform.position = positionA;
    }
}
