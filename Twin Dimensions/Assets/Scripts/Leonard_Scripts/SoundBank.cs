using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SoundBank : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;

    public static SoundBank instance = null;       

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;
    
    [FoldoutGroup("PlayerSFX")][ShowInInspector] public static AudioClip[] playerWalking;
    [FoldoutGroup("PlayerSFX")][ShowInInspector] public static AudioClip[] playerPunching;
    [FoldoutGroup("PlayerSFX")][ShowInInspector] public static AudioClip[] playerSummoning;
    [FoldoutGroup("PlayerSFX")][ShowInInspector] public static AudioClip[] playerTeleporting;
    [FoldoutGroup("PlayerSFX")][ShowInInspector] public static AudioClip[] playerDeath;
    
    [FoldoutGroup("EnvironmentalSFX")][ShowInInspector] public static AudioClip[] openingDoor;
    [FoldoutGroup("EnvironmentalSFX")][ShowInInspector] public static AudioClip[] spikesRaised;
    [FoldoutGroup("EnvironmentalSFX")][ShowInInspector] public static AudioClip[] spikesLowering;
    [FoldoutGroup("EnvironmentalSFX")][ShowInInspector] public static AudioClip[] pressurePlateActivation;
    [FoldoutGroup("EnvironmentalSFX")][ShowInInspector] public static AudioClip[] pressurePlateDeactivation;

    [FoldoutGroup("AmbientSFX")][ShowInInspector] public static AudioClip[] windSFX;
    [FoldoutGroup("AmbientSFX")][ShowInInspector] public static AudioClip[] waterSFX;
    [FoldoutGroup("AmbientSFX")][ShowInInspector] public static AudioClip[] grassSFX;
    
    [FoldoutGroup("ElephantSFX")][ShowInInspector] public static AudioClip[] elephantCharge;
    [FoldoutGroup("ElephantSFX")][ShowInInspector] public static AudioClip[] elephantStartCharge;
    [FoldoutGroup("ElephantSFX")][ShowInInspector] public static AudioClip[] elephantDeath;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;

        efxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = UnityEngine.Random.Range(0, clips.Length);

        float randomPitch = UnityEngine.Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;

        efxSource.clip = clips[randomIndex];

        efxSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        MonitorSFX();
    }

    void MonitorSFX()
    {
        //PLAYER SFX
        if(PlayerController.playerIsMoving) RandomizeSfx(playerWalking);
        if(TeleportationManager.isTeleporting) RandomizeSfx(playerTeleporting);
        if(StatueManager.isPlacingStatue) RandomizeSfx(playerSummoning);
        if(StatueManager.isPunchingStatue) RandomizeSfx(playerPunching);
        if(GameMaster.playerIsDead) RandomizeSfx(playerDeath);

        //ELEPHANT SFX

    }
}
