using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class PickUpSand : MonoBehaviour
{
    [FoldoutGroup("Sand SFX")][SerializeField] AudioClip HourglassFillingSound;

    void OnTriggerEnter2D(Collider2D col)
    {
        NumOfHourglasses.numOfHourglasses ++;
        Destroy(gameObject);
    }
}
