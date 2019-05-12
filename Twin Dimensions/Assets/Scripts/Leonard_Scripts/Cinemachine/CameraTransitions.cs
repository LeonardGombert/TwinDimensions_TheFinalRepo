using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using Cinemachine;
using Cinemachine.Editor;

public class CameraTransitions : MonoBehaviour
{
    public static CameraTransitions instance;

    CinemachineVirtualCamera playerCamera; 
    CinemachineVirtualCamera generalZoomCamera;
    CinemachineVirtualCamera relativeZoomCamera;
    CinemachineVirtualCamera otherWorldCam;

    [FoldoutGroup("Virtual Camera World 1 References")][SerializeField]
    CinemachineVirtualCamera world1PlayerCamera;
    [FoldoutGroup("Virtual Camera World 1 References")][SerializeField]
    CinemachineVirtualCamera world1RelativeZoom;
    [FoldoutGroup("Virtual Camera World 1 References")][SerializeField]
    CinemachineVirtualCamera world1GeneralZoom;

    [FoldoutGroup("Virtual Camera World 2 References")][SerializeField]
    CinemachineVirtualCamera world2Player2Camera;    
    [FoldoutGroup("Virtual Camera World 2 References")][SerializeField]
    CinemachineVirtualCamera wrld2RelativeZoom;
    [FoldoutGroup("Virtual Camera World 2 References")][SerializeField]
    CinemachineVirtualCamera wrld2GeneralZoom;
    
    [FoldoutGroup("Virtual Camera World 1 References")][SerializeField]
    CinemachineVirtualCamera exitCamera;

    float timeHeldDown;
    float minTimeToHoldDown = 7f;

    void Awake()
    {
        if(instance == null)
        {            
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerLevelView();
        //PlayerLookAtOtherWorld();
    }

    private void PlayerLevelView()
    {
        if(LayerManager.PlayerIsInRealWorld())
        {
            playerCamera = world1PlayerCamera;
            relativeZoomCamera = world1RelativeZoom;
            generalZoomCamera = world1GeneralZoom;
        }
        
        else if(!LayerManager.PlayerIsInRealWorld())
        {
            playerCamera = world2Player2Camera;
            relativeZoomCamera = wrld2RelativeZoom;
            generalZoomCamera = wrld2GeneralZoom;
        }

        if(PlayerInputManager.instance.GetKey("relativeZoom"))
        {
            if(timeHeldDown >= minTimeToHoldDown && !PlayerController.isMoving)
            {
                relativeZoomCamera.gameObject.SetActive(false);
                exitCamera.gameObject.SetActive(true);
            }

            else
            {
                playerCamera.gameObject.SetActive(false);
                exitCamera.gameObject.SetActive(false);
                relativeZoomCamera.gameObject.SetActive(true);

                timeHeldDown += Time.fixedUnscaledDeltaTime;
            }       
        }

        if(PlayerInputManager.instance.GetKeyUp("relativeZoom"))
        {
            playerCamera.gameObject.SetActive(true);
            relativeZoomCamera.gameObject.SetActive(false);
            timeHeldDown = 0;
        }

        if(PlayerInputManager.instance.GetKey("generalZoom"))
        {
            playerCamera.gameObject.SetActive(false);
            generalZoomCamera.gameObject.SetActive(true);
        }

        if(PlayerInputManager.instance.GetKeyUp("generalZoom"))
        {
            playerCamera.gameObject.SetActive(true);
            generalZoomCamera.gameObject.SetActive(false);
        }
    }

    public static void ChangingWorlds(CinemachineVirtualCamera cam)
    {
        Transform player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cam.m_Follow = player;

        cam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
    }

    public static void ChangingWorldsBack(CinemachineVirtualCamera cam)
    {
        cam.m_Follow = default;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
    }

    public static void ChangeCameraOnce(CinemachineVirtualCamera cam1, CinemachineVirtualCamera cam2)
    {
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
    }

    public static void ChangeCameraBack(CinemachineVirtualCamera cam1, CinemachineVirtualCamera cam2)
    {
        cam1.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);        
    }
}