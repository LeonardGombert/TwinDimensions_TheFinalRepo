using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SandDoor : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject paySystemUI;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }
    }

    public void Resume()
    {
        paySystemUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        paySystemUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Dialog");
        SandTextScript.sandAmount = SandTextScript.sandAmount - 2;
        Debug.Log("J'ai " + SandTextScript.sandAmount + " sables");
    }
}
