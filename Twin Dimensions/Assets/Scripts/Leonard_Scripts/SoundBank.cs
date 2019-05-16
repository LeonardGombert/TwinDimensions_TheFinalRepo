using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SoundBank : MonoBehaviour
{
    [ShowInInspector] public static AudioClip[] playerWalking;
    [ShowInInspector] public static AudioClip[] playerPunching;
    [ShowInInspector] public static AudioClip[] playerSummoning;
    [ShowInInspector] public static AudioClip[] playerTeleporting;
    [ShowInInspector] public static AudioClip[] playerDeath;   

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
