using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance = null;
    //public Text Enemies;
    public Text time;
    //public Text death;
    public Text reset;
    public Text FinalGrade;
    public Text Grade;
    public int timeCap1;
    public int timeCap2;
    public int resetCap1;
    public int resetCap2;

    bool CalculateGrade1 = true;
    bool CalculateGrade2 = true;
    [ShowInInspector] public static int FinalScore;
    //[ShowInInspector] public static int FinalScore1;

    [FoldoutGroup("DEBUG Stats")][SerializeField] int AmountOfEnemiesAtStart;
    [FoldoutGroup("DEBUG Stats")][SerializeField] int AmountOfEnemiesAtEnd;
    [FoldoutGroup("DEBUG Stats")][SerializeField] int amountOfKills;
    [FoldoutGroup("DEBUG Stats")][SerializeField] int timeToComplete;
    [FoldoutGroup("DEBUG Stats")][SerializeField] int roomResets;
    [FoldoutGroup("DEBUG Stats")][SerializeField] int playerDeaths;
    [FoldoutGroup("DEBUG Stats")][ShowInInspector] public static bool playerCompletedLevel;
    [FoldoutGroup("DEBUG Stats")][SerializeField] List<GameObject> enemiesInRoom = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
            
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
        enemiesInRoom.Clear();

        enemiesInRoom.AddRange(GameObject.FindGameObjectsWithTag("Elephant"));
        enemiesInRoom.AddRange(GameObject.FindGameObjectsWithTag("Firebreather"));
        enemiesInRoom.AddRange(GameObject.FindGameObjectsWithTag("ActivationPriest"));
        //enemiesInRoom.AddRange(GameObject.FindGameObjectsWithTag("Priest"));
        enemiesInRoom.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        
        AmountOfEnemiesAtStart = (int)enemiesInRoom.Count;
    }
    
    void Update()
    {
        if(LevelManager.playerCompletedLevel)
        {
            AmountOfEnemiesAtEnd = enemiesInRoom.Count;
            amountOfKills = AmountOfEnemiesAtStart - AmountOfEnemiesAtEnd;
            ConvertValues(amountOfKills, roomResets, playerDeaths);
        }

        PlayerDied();

        CalculateGrade();
    }
    
    void ConvertValues(int kills, int resets, int deaths)
    {
        amountOfKills = kills;
        timeToComplete = (int)Time.timeSinceLevelLoad;
        roomResets = resets;
        playerDeaths = deaths;
    }  

    void WasKilled(GameObject killedObject)
    {
        enemiesInRoom.Remove(killedObject);
    }

    void PlayerResetRoom()
    {
        ++roomResets;
        Debug.Log(roomResets);
    }

    void PlayerDied()
    {
        if (PlayerController.playerIsDead)
        {
            ++playerDeaths;
            PlayerController.playerIsDead = false;
            Debug.Log(playerDeaths);
        }
        else return;
    }
    
    void CalculateGrade()
    {
        //Enemies.text = "ENEMIES KILLED " + amountOfKills;
        time.text = "TIME TAKEN: " + timeToComplete;
        //death.text = "RESETS " + playerDeaths;
        reset.text = "RESETS DONE: " + roomResets;
        FinalGrade.text = "YOUR FINAL GRADE IS : ";

        if (CalculateGrade1 == true && LevelManager.playerCompletedLevel == true && (roomResets + playerDeaths) < resetCap1)
        {
            FinalScore += 3;
            CalculateGrade1 = false;
        }

        if (CalculateGrade1  && LevelManager.playerCompletedLevel  && (roomResets + playerDeaths) > resetCap1)
        {
            FinalScore += 2;
            CalculateGrade1 = false;
        }

        if (CalculateGrade1 == true && LevelManager.playerCompletedLevel == true && (roomResets + playerDeaths) > resetCap2)
        {
            FinalScore += 1;
            CalculateGrade1 = false;
        } 
        
        if (CalculateGrade2 == true && LevelManager.playerCompletedLevel == true && timeToComplete < timeCap1)
        {
            FinalScore += 3;
            CalculateGrade2 = false;
        }

        if (CalculateGrade2 == true && LevelManager.playerCompletedLevel == true && timeToComplete > timeCap1)
        {
            FinalScore += 2;
            CalculateGrade2 = false;
        }

        if (CalculateGrade2 == true && LevelManager.playerCompletedLevel == true &&  timeToComplete > timeCap2)
        {
            FinalScore += 1;
            CalculateGrade2 = false;
        }

        if (FinalScore == 6)
        {
            Grade.text = "A";
        }
        if (FinalScore == 5 || FinalScore == 4)
        {
            Grade.text = "B";
        }
        if (FinalScore == 3 || FinalScore == 2)
        {
            Grade.text = "C";
        }
        if (FinalScore == 1 || FinalScore == 0)
        {
            Grade.text = "D";
        }
    }
}