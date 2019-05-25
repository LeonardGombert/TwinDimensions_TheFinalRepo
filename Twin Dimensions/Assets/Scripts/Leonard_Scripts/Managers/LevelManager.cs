using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

public class LevelManager : SerializedMonoBehaviour
{
    public GameObject EndPanel;



   
    [ShowInInspector] public static bool playerCompletedLevel = false;
    //public int index;
    //public string levelName;
    Image black;
    Animator anim;

    public static object Instance { get; internal set; }

    // Start is called before the first frame update
    void Awake()
    {
        black = GameObject.FindGameObjectWithTag("LevelFadeAnim").GetComponent<Image>();
        anim = GameObject.FindGameObjectWithTag("LevelFadeAnim").GetComponent<Animator>();
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

    public void ReachedExit()
    {
        playerCompletedLevel = true;
        StartCoroutine(Fading());
        EndPanel.SetActive(true);
    }
}
