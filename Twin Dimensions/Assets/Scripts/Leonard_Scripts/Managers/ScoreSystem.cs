using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;
    
    [FoldoutGroup("DEBUG Stats")][SerializeField] int AmountOfEnemiesAtStart;
    [FoldoutGroup("DEBUG Stats")][SerializeField] int AmountOfEnemiesAtEnd;
    [FoldoutGroup("DEBUG Stats")][SerializeField] int amountOfKills;
    [FoldoutGroup("DEBUG Stats")][SerializeField] int timeToComplete;
    [FoldoutGroup("DEBUG Stats")][SerializeField] int roomResets;
    [FoldoutGroup("DEBUG Stats")][SerializeField] int playerDeaths;
    [FoldoutGroup("DEBUG Stats")][ShowInInspector] public static bool playerCompletedLevel;
    [FoldoutGroup("DEBUG Stats")][SerializeField] List<GameObject> enemiesInRoom = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        if(LevelManager.playerCompletedLevel)
        {
            AmountOfEnemiesAtEnd = enemiesInRoom.Count;
            amountOfKills = AmountOfEnemiesAtStart - AmountOfEnemiesAtEnd;
            ConvertValues(amountOfKills, roomResets, playerDeaths);
        }
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
        ++playerDeaths;
        Debug.Log(playerDeaths);
    }
}