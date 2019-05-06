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
    Camera levelCamera;

    [FoldoutGroup("Virtual Camera World 1 References")][SerializeField]
    CinemachineVirtualCamera player1Camera;
    [FoldoutGroup("Virtual Camera World 1 References")][SerializeField]
    Camera level1Camera;
    [FoldoutGroup("Virtual Camera World 2 References")][SerializeField]
    CinemachineVirtualCamera player2Camera;
    [FoldoutGroup("Virtual Camera World 2 References")][SerializeField]
    Camera level2Camera;

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

        //player1Camera = GameObject.FindGameObjectWithTag("Player Virtual Camera 1").GetComponent<CinemachineVirtualCamera>();
        //level1Camera = GameObject.FindGameObjectWithTag("Level Virtual Camera 1").GetComponent<Camera>();
        //player2Camera = GameObject.FindGameObjectWithTag("Player Virtual Camera 2").GetComponent<CinemachineVirtualCamera>();
        //level2Camera = GameObject.FindGameObjectWithTag("Level Virtual Camera 2").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerLevelView();
    }

    private void PlayerLevelView()
    {
        if(LayerManager.PlayerIsInRealWorld())
        {
            playerCamera = player1Camera;
            levelCamera = level1Camera;
        }
        else if(!LayerManager.PlayerIsInRealWorld())
        {
            playerCamera = player2Camera;
            levelCamera = level2Camera;     
        }
        
        if(PlayerInputManager.instance.GetKey("cameraZoomOut"))
        {
            playerCamera.gameObject.SetActive(false);
            levelCamera.gameObject.SetActive(true);
        }
        
        else 
        {
            playerCamera.gameObject.SetActive(true);
            levelCamera.gameObject.SetActive(false);
        }
    }

    public static void ChangingWorlds(CinemachineVirtualCamera cam)
    {
        Transform player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cam.m_Follow = player;

        cam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 20;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 20;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 20;
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
