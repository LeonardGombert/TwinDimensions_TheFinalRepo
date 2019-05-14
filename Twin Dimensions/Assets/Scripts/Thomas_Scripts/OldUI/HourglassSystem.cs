using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourglassSystem : MonoBehaviour
{
    public int hourglasses;
    public int numOfHourgasses;

    public Image[] picHourglasses;
    public Sprite fullHourglass;
    public Sprite emptyHourglass;

    void Update()
    {
        if(hourglasses > numOfHourgasses)
        {
             hourglasses = numOfHourgasses;
        }
        if(hourglasses < 0)
        {
            hourglasses = 0;
        }


        for (int i = 0; i < picHourglasses.Length;  i++)
        {
            if(i < hourglasses)
        {
                picHourglasses[i].sprite = fullHourglass;
        }
        else 
       {       
           picHourglasses[i].sprite = emptyHourglass;
        }
            
           if(i < hourglasses)
           {
                picHourglasses[i].enabled = true;
            }
            else
            {
                picHourglasses[i].enabled = false;
            }
                
        }
    }
}
