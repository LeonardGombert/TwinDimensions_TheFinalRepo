using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine.Rendering.PostProcessing;

public class Teleportation : SerializedMonoBehaviour
{
    #region Variable Declarations

    #region //TELEPORTATION
    public Tile highlightTile;
    public Tilemap highlightTilemap;

    private Vector3 mousePosition;
    private Vector3Int previous;

    public Camera world1Cam;
    public Camera world2Cam;

    bool isTeleporting = false;
    public static bool hasTeleported = false;

    Animator anim;
    #endregion

    #region //ENEMY SELECTION
    [FoldoutGroup("Selected Enemies")][SerializeField]
    List<GameObject> SelectedEnemies = new List<GameObject>();

    private GameObject targetedObject1;
    private GameObject targetedObject2;
    private GameObject targetedChildObject;
    [FoldoutGroup("Selected Enemies")][SerializeField]
    private GameObject lightningPrefab;

    bool isSelectingEnemies = false;
    bool hasSelectedEnemies = false;

    bool playerHasSelectedFirstEnemy = false;
    bool playerHasSelectedSecondEnemy = false;
    #endregion

    #region //COUNTDOWN TIMER
    [FoldoutGroup("Time Variables")][SerializeField]
    public float teleportTimer;
    [FoldoutGroup("Time Variables")][SerializeField]
    public float baseCountdownTimerValue;
    [FoldoutGroup("Time Variables")][SerializeField]
    public float slowTimeMultiplier;
    #endregion

    #region //POST PROCESSING
    [FoldoutGroup("Effect Values")][SerializeField]
    private float maxBloomIntensity;
    [FoldoutGroup("Effect Values")][SerializeField]
    private float maxBloomSoftKnee;
    [FoldoutGroup("Effect Values")][SerializeField]
    private float maxBloomDiffusion;    
    [FoldoutGroup("Effect Values")][SerializeField]
    private float maxBloomAnamorphRatio;    

    [FoldoutGroup("Effect Values")][SerializeField]
    private float maxChrmAberrationIntensity;

    [FoldoutGroup("Effect Values")][SerializeField]
    private float maxLensDistortionIntensity;
    [FoldoutGroup("Effect Values")][SerializeField]
    private float maxLensDistortionScale;
    
    [FoldoutGroup("Lerp Values in Seconds")][SerializeField]
    private float startTime;
    [FoldoutGroup("Lerp Values in Seconds")][SerializeField]
    private float timeToTeleport;
    
    public List<PostProcessVolume> m_VolumeList = new List<PostProcessVolume>();

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
    }

    private void Update()
    {
        CheckPlayerInputs();
                
        SelectEnemies(highlightTilemap);

        TeleportCountdownTimer();

        TeleportationPostProcessing();
    }
    #endregion

    #region Teleportation Functions

    private void CheckPlayerInputs()
    {
        if (PlayerInputManager.instance.KeyDown("selectEnemies"))
        {
            isSelectingEnemies = true;
            hasSelectedEnemies = true;
        }

        if (PlayerInputManager.instance.KeyDown("teleport") && hasSelectedEnemies == true)
        {
            isSelectingEnemies = false;
            if (isOnLockedLayer == false) isTeleporting = true;
            else if (isOnLockedLayer == true) isTeleporting = false; Debug.Log("I am unable to Teleport");
        }
    }

    #region //SELECT ENEMIES
    public void SelectEnemies(Tilemap currentTilemap)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = ExtensionMethods.getFlooredWorldPosition(mousePosition);

        Vector3Int currentMousePositionInGrid = currentTilemap.WorldToCell(mousePosition);

        if (isSelectingEnemies == true)
        {
            Time.timeScale = slowTimeMultiplier;

            if (currentMousePositionInGrid != previous)
            {
                currentTilemap.SetTile(currentMousePositionInGrid, highlightTile);

                currentTilemap.SetTile(previous, null);

                previous = currentMousePositionInGrid;
            }
        }

        else if (isSelectingEnemies == false)
        {
            Time.timeScale = 1.0f;

            currentTilemap.SetTile(previous, null);
            currentTilemap.SetTile(currentMousePositionInGrid, null);
        }

        if (Input.GetMouseButtonDown(0) && playerHasSelectedFirstEnemy == false) //Select Enemy1
        {
            RaycastHit2D firstMouseCollisionRay = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

            if (firstMouseCollisionRay.collider != null)
            {
                Instantiate(lightningPrefab, mousePosition, Quaternion.identity);

                targetedObject1 = firstMouseCollisionRay.collider.gameObject;

                SelectedEnemies.Add(targetedObject1);

                Debug.Log("Object no1 is: " + targetedObject1);

                playerHasSelectedFirstEnemy = true;
            }
        }

        if (Input.GetMouseButtonDown(1) && playerHasSelectedFirstEnemy == true) //Select Enemy2
        {
            RaycastHit2D secondMouseCollisionRay = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

            if (secondMouseCollisionRay.collider != null)
            {
                Instantiate(lightningPrefab, mousePosition, Quaternion.identity);

                targetedObject2 = secondMouseCollisionRay.collider.gameObject;
                SelectedEnemies.Add(targetedObject2);
                Debug.Log("Object no2 is: " + targetedObject2);

                playerHasSelectedSecondEnemy = true;
                isSelectingEnemies = false;
            }
        }
        hasSelectedEnemies = true;
    }
    #endregion

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
                    SwitchWorlds(SelectedEnemies);
                    teleportTimer = baseCountdownTimerValue;
                }
            }
        }

        else if (isTeleporting == false) anim.SetBool("isTeleporting", false); return;
    }

    private void SwitchWorlds(List<GameObject> EnemiesToTeleport)
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

        //ENEMY TELEPORTATION
        foreach (GameObject Enemy in EnemiesToTeleport)
        MonsterClass.isBeingTeleported = true;

        EnemiesToTeleport.Clear();

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