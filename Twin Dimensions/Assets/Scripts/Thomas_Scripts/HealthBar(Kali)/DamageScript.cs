using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        HealthBarScript.health -= 10f;
    }
}


// A adapter pour chaque coup donné a Kali