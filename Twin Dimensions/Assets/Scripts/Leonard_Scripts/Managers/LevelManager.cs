using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

public class LevelManager : MonoBehaviour
{
    public GameObject EndPanel;
    [ShowInInspector] public static bool playerCompletedLevel = false;
    //public int index;
    //public string levelName;
    Image black;
    Animator anim;

    public static object Instance { get; internal set; }
    public object FinalScore { get; private set; }

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
        if(Input.GetKeyDown(KeyCode.X)) LoadNext();
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

        Debug.Log(FinalScore);       
    }

    public void ReachedExit()
    {
        playerCompletedLevel = true;
        EndPanel.SetActive(true);
    }
    
    void LoadNext()
    {       
        StartCoroutine(Fading());
    }
}
