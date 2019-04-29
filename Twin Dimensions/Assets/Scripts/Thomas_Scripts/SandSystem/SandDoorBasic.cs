using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SandDoorBasic : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
            if(SandTextScript.sandAmount >= 2)
                {
                SceneManager.LoadScene("Menu");
                }
    }
}
