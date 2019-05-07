using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class LevelExit : SerializedMonoBehaviour
{
    Animation doorOpenAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            doorOpenAnim.playAutomatically = true;
            new WaitForEndOfFrame();
            PlayerIsExiting();
        }
    }

    void PlayerIsExiting()
    {
        //SceneManager.LoadScene();
    }
}
