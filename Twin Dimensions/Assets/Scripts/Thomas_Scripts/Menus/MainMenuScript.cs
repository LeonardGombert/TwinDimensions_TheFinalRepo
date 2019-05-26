using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public string nameOfTheScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(nameOfTheScene);
    }
   
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit(); 

    }

}
