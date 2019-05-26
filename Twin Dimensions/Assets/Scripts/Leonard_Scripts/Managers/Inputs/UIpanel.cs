using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIpanel : MonoBehaviour
{
    //public Text Enemies;
    public Text Time;
    //public Text Deaths;
    public Text Resets;
    //public int EnemiesCap;
    public int TimeCap;
    //public int DeathCap;
    public int ResetCap;

    public GameObject Panel;
    int counter;


    private void Start()
    {
        
        //Enemies.text = "ENEMIES TO BE KILLED " + EnemiesCap;
        Time.text = "TIME FOR BEST GRADE " + TimeCap + " MINS";
        //Deaths.text = "RESETS CAP FOR BEST GRADE " + DeathCap;
        Resets.text = "DEATHS CAP FOR BEST GRADE " + ResetCap;
    }

    public void showhidePanel ()
    {
        counter++;
        if (counter % 2 == 1)
        {
            Panel.gameObject.SetActive(false);
        }
        else
        {
            Panel.gameObject.SetActive(true);
        }

    }


}
