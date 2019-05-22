﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class LevelManager : SerializedMonoBehaviour
{
    [ShowInInspector] public static bool playerCompletedLevel = false;
    //public int index;
    //public string levelName;
    public Image black;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerInputManager.instance.GetKeyDown("resetScene")) StartCoroutine(Fading());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player") StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(()=>black.color.a==1);
        
        if (playerCompletedLevel)
        {
            playerCompletedLevel = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadNextLevel()
    {
    }
}
