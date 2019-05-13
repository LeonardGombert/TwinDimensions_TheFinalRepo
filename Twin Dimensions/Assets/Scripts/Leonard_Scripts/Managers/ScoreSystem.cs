using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    int amountOfKills;
    int timeToComplete;
    int roomResets;
    int playerDeaths;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //on game end, run ConvertValues();
    }
    
    void ConvertValues(int kills, int resets, int deaths)
    {
        amountOfKills = kills;
        timeToComplete = (int)Time.timeSinceLevelLoad;
        roomResets = resets;
        playerDeaths = deaths;
    }

    void CalculateGrade()
    {

    }
}
