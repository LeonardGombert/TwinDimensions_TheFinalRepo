using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelection : MonoBehaviour
{
    public static LanguageSelection instance;

    public static bool frenchDialogue = false;
    public static bool englishDialogue = true;
    
    // Start is called before the first frame update
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
        
    }
}
