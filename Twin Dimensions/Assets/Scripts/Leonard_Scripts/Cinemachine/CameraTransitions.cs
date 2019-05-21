using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using Cinemachine;
using Cinemachine.Editor;

public class CameraTransitions : MonoBehaviour
{
    CinemachineVirtualCamera playerCamera; 
    CinemachineVirtualCamera generalZoomCamera;
    CinemachineVirtualCamera relativeZoomCamera;
    CinemachineVirtualCamera otherWorldCam;

    [FoldoutGroup("Yes")][SerializeField] Cinemachine.NoiseSettings.NoiseParams yes;

    [FoldoutGroup("Virtual Camera World 1 References")][SerializeField]
    CinemachineVirtualCamera world1PlayerCamera;
    [FoldoutGroup("Virtual Camera World 1 References")][SerializeField]
    CinemachineVirtualCamera world1RelativeZoom;
    [FoldoutGroup("Virtual Camera World 1 References")][SerializeField]
    CinemachineVirtualCamera world1GeneralZoom;

    [FoldoutGroup("Virtual Camera World 2 References")][SerializeField]
    CinemachineVirtualCamera world2Player2Camera;    
    [FoldoutGroup("Virtual Camera World 2 References")][SerializeField]
    CinemachineVirtualCamera world2RelativeZoom;
    [FoldoutGroup("Virtual Camera World 2 References")][SerializeField]
    CinemachineVirtualCamera world2GeneralZoom;
    
    [FoldoutGroup("Virtual Camera World 1 References")][SerializeField]
    CinemachineVirtualCamera exitCamera;

    float timeHeldDown;
    float minTimeToHoldDown = 7f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameras();
        PlayerLevelView();        
    }

    void UpdateCameras()
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
            relativeZoomCamera = world2RelativeZoom;
            generalZoomCamera = world2GeneralZoom;
        }
    }

    private void PlayerLevelView()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {            
            if(PlayerInputManager.instance.GetKey("relativeZoom"))
            {
                if(timeHeldDown >= minTimeToHoldDown && !PlayerController.playerIsMoving)
                {
                    relativeZoomCamera.gameObject.SetActive(false);
                    exitCamera.gameObject.SetActive(true);
                }

                else
                {
                    playerCamera.gameObject.SetActive(false);
                    exitCamera.gameObject.SetActive(false);
                    relativeZoomCamera.gameObject.SetActive(true);

                    timeHeldDown += Time.deltaTime;
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

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            playerCamera.gameObject.SetActive(true);
            generalZoomCamera.gameObject.SetActive(false);
            relativeZoomCamera.gameObject.SetActive(false);
            timeHeldDown = 0;
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

    public static void ScreenshakeOnCharge(CinemachineVirtualCamera cam2, Cinemachine.NoiseSettings.NoiseParams noiseCam)
    {
        noiseCam.Amplitude = 0.5f;
        noiseCam.Frequency = 9;
        //Serializse Impulse --> Impulse.GenerateImpuse();
    }
}