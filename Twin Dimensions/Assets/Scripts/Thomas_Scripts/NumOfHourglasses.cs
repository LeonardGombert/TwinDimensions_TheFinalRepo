using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NumOfHourglasses : MonoBehaviour
{
    Text text;
    public static int numOfHourglasses;
    public int limitHourglasses;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = numOfHourglasses.ToString();


        if(numOfHourglasses > limitHourglasses)
        {
            numOfHourglasses = limitHourglasses;
        }

        if(numOfHourglasses < 0)
        {
            numOfHourglasses = 0;
        }
    }
}

// Avec gestion passant par l'UI, a voir si conservé (DontDestroyOnLoad a récupérer)