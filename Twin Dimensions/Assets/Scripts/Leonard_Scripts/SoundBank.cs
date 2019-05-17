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
    
    [FoldoutGroup("PlayerSFX")][ShowInInspector] AudioClip[] playerWalking;
    [FoldoutGroup("PlayerSFX")][ShowInInspector] AudioClip[] playerPunching;
    [FoldoutGroup("PlayerSFX")][ShowInInspector] AudioClip[] playerSummoning;
    [FoldoutGroup("PlayerSFX")][ShowInInspector] AudioClip[] playerTeleporting;
    [FoldoutGroup("PlayerSFX")][ShowInInspector] AudioClip[] playerDeath;
    
    [FoldoutGroup("EnvironmentalSFX")][ShowInInspector] AudioClip[] openingDoor;
    [FoldoutGroup("EnvironmentalSFX")][ShowInInspector] AudioClip[] spikesRaised;
    [FoldoutGroup("EnvironmentalSFX")][ShowInInspector] AudioClip[] spikesLowering;
    [FoldoutGroup("EnvironmentalSFX")][ShowInInspector] AudioClip[] pressurePlateActivation;
    [FoldoutGroup("EnvironmentalSFX")][ShowInInspector] AudioClip[] pressurePlateDeactivation;

    [FoldoutGroup("AmbientSFX")][ShowInInspector] AudioClip[] windSFX;
    [FoldoutGroup("AmbientSFX")][ShowInInspector] AudioClip[] waterSFX;
    [FoldoutGroup("AmbientSFX")][ShowInInspector] AudioClip[] grassSFX;
    
    [FoldoutGroup("ElephantSFX")][ShowInInspector] AudioClip[] elephantCharge;
    [FoldoutGroup("ElephantSFX")][ShowInInspector] AudioClip[] elephantStartCharge;
    [FoldoutGroup("ElephantSFX")][ShowInInspector] AudioClip[] elephantDeath;

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
