using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool playerIsDead = false;

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
        CheckPlayerState();
    }

    void CheckPlayerState()
    {
        if(playerIsDead == true)
        {
            Debug.Log("Player died");
            //Death Animation
            //Death Screen
            //Reload Scene
        }
    }
}
