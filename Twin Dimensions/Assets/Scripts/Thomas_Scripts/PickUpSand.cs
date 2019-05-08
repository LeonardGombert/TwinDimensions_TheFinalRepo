using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSand : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        GetComponent<HourglassSystem.hourglasses>();
    
        Destroy(gameObject);

    }
}
