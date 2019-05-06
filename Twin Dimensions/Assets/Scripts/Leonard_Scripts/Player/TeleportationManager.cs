using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine.Rendering.PostProcessing;

public class TeleportationManager : SerializedMonoBehaviour
{
    #region Variable Declarations

    #region //TELEPORTATION
    [FoldoutGroup("Base References")][SerializeField]
    Tile highlightTile;
    [FoldoutGroup("Base References")][SerializeField]
    Tilemap movementTilemap;

    Vector3 mousePosition;
    Vector3Int previous;

    [FoldoutGroup("Cameras")][SerializeField]
    Camera world1Cam;
    [FoldoutGroup("Cameras")][SerializeField]
    Camera world2Cam;

    bool isTeleporting = false;
    public static bool hasTeleported = false; //avoids looping the Teleport to hook function

    Animator anim;
    #endregion

    #region //COUNTDOWN TIMER
    [FoldoutGroup("Time Variables")][SerializeField]
    float teleportTimer;
    [FoldoutGroup("Time Variables")][SerializeField]
    float baseCountdownTimerValue;
    [FoldoutGroup("Time Variables")][SerializeField]
    float slowTimeMultiplier;
    #endregion

    #region //POST PROCESSING
    [FoldoutGroup("Post Process Effect Values")][SerializeField]
    float maxBloomIntensity;
    [FoldoutGroup("Post Process Effect Values")][SerializeField]
    float maxBloomSoftKnee;
    [FoldoutGroup("Post Process Effect Values")][SerializeField]
    float maxBloomDiffusion;    
    [FoldoutGroup("Post Process Effect Values")][SerializeField]
    float maxBloomAnamorphRatio;
    [FoldoutGroup("Post Process Effect Values")][SerializeField]
    float maxChrmAberrationIntensity;
    [FoldoutGroup("Post Process Effect Values")][SerializeField]
    float maxLensDistortionIntensity;
    [FoldoutGroup("Post Process Effect Values")][SerializeField]
    float maxLensDistortionScale;
    
    [FoldoutGroup("Post Process Profiles")][SerializeField]
    List<PostProcessVolume> m_VolumeList = new List<PostProcessVolume>();

    [FoldoutGroup("Lerp Values in Seconds")][SerializeField]
    float startTime;
    [FoldoutGroup("Lerp Values in Seconds")][SerializeField]
    float timeToTeleport;

    Bloom bloomLayer = null;
    ChromaticAberration chrmAberrationLayer = null;
    LensDistortion lensDistortionLayer = null;
    #endregion

    #region //LOCK PRIEST

    public static bool isOnLockedLayer = false;

    #endregion

    #endregion
    
    #region Monobehavior Callbacks

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movementTilemap = GameObject.FindGameObjectWithTag("Movement Tilemap").GetComponent<Tilemap>();
    }

    private void Update()
    {
        CheckPlayerInputs();

        TeleportCountdownTimer();

        TeleportationPostProcessing();
    }
    #endregion

    #region Teleportation Functions

    private void CheckPlayerInputs()
    {

        if (PlayerInputManager.instance.GetKeyDown("teleport"))
        {
            if (isOnLockedLayer == false) isTeleporting = true;
            else if (isOnLockedLayer == true) isTeleporting = false; Debug.Log("I am unable to Teleport");
        }
    }

    #region //TELEPORT TO OTHER WORLD
    private void TeleportCountdownTimer()
    {
        if (isTeleporting == true)
        {
            anim.SetBool("isTeleporting", true);

            if (teleportTimer > 0)
            {
                teleportTimer -= Time.fixedUnscaledDeltaTime;

                if (teleportTimer <= 0)
                {
                    SwitchWorlds();
                    teleportTimer = baseCountdownTimerValue;
                }
            }
        }

        else if (isTeleporting == false) anim.SetBool("isTeleporting", false); return;
    }

    private void SwitchWorlds()
    {
        //PLAYER TELEPORTATION
        if (LayerManager.PlayerIsInRealWorld())
        {
            world1Cam.gameObject.SetActive(false);
            world2Cam.gameObject.SetActive(true);

            gameObject.layer = LayerMask.NameToLayer("Player Layer 2");
            hasTeleported = true;
        }

        else if (!LayerManager.PlayerIsInRealWorld())
        {
            world1Cam.gameObject.SetActive(true);
            world2Cam.gameObject.SetActive(false);

            gameObject.layer = LayerMask.NameToLayer("Player Layer 1");
        }

        isTeleporting = false;
    }

    private void TeleportationPostProcessing()
    {
        foreach (PostProcessVolume m_Volume in m_VolumeList)
        {
            m_Volume.profile.TryGetSettings(out bloomLayer);
            m_Volume.profile.TryGetSettings(out chrmAberrationLayer);
            m_Volume.profile.TryGetSettings(out lensDistortionLayer);

            if (startTime <= timeToTeleport && isTeleporting == true)
            {
                startTime += Time.deltaTime;

                //Bloom Values
                bloomLayer.enabled.value = true;
                bloomLayer.intensity.value = Mathf.Lerp(0f, maxBloomIntensity, startTime / timeToTeleport);
                bloomLayer.softKnee.value = Mathf.Lerp(0f, maxBloomSoftKnee, startTime / timeToTeleport);
                bloomLayer.diffusion.value = Mathf.Lerp(1f, maxBloomDiffusion, startTime / timeToTeleport);
                bloomLayer.anamorphicRatio.value = Mathf.Lerp(0f, maxBloomAnamorphRatio, startTime / timeToTeleport);
                
                //Chromatic Aberration Values
                chrmAberrationLayer.enabled.value = true;
                chrmAberrationLayer.intensity.value = Mathf.Lerp(0f, maxChrmAberrationIntensity, startTime / timeToTeleport);

                //Lens Distortion Values
                lensDistortionLayer.enabled.value = true;
                lensDistortionLayer.intensity.value = Mathf.Lerp(0f, maxLensDistortionIntensity, startTime / timeToTeleport);
                lensDistortionLayer.scale.value = Mathf.Lerp(1f, maxLensDistortionScale, startTime / timeToTeleport);
            }

            else if (isTeleporting == false)
            {
                startTime = 0f;

                bloomLayer.enabled.value = false;
                chrmAberrationLayer.enabled.value = false;
                lensDistortionLayer.enabled.value = false;
            }
        }
    }
    #endregion
    #endregion
}