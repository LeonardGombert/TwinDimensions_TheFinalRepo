using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    public KeybindingManager keybindingManager;

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

    public bool KeyDown(string key)
    {
        if(Input.GetKey(keybindingManager.CheckKey(key)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
