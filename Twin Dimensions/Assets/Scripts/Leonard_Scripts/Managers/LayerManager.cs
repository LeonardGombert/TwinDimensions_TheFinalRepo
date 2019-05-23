using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class LayerManager : SerializedMonoBehaviour
{    
    //[FoldoutGroup("Enemies in Scene")][SerializeField]
    //List <GameObject> fillThisListWithEnemies;
    
    GameObject player;    

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        //fillThisListWithEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        //fillThisListWithEnemies.AddRange(GameObject.FindGameObjectsWithTag("Elephant"));
        //fillThisListWithEnemies.AddRange(GameObject.FindGameObjectsWithTag("Firebreather"));
        //fillThisListWithEnemies.AddRange(GameObject.FindGameObjectsWithTag("ActivationPriest"));
    }

    // Update is called once per frame
    void Update()
    {
        //CompareEnemyToPlayerLayer(fillThisListWithEnemies);
    }

    public static void LayerSwitchManager(GameObject gameObject, string layerName) //changes all children objects to same layer as parent
    {
        if (gameObject == null) return;

        Transform[] childList = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in childList)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    public static bool PlayerIsInRealWorld() //checks if player is on level 1
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player.gameObject.layer == LayerMask.NameToLayer("Player Layer 1")) return true;

        if(player.gameObject.layer == LayerMask.NameToLayer("Player Layer 2")) return false;

        else return false;
    }

    public static bool EnemyIsInRealWorld(GameObject enemy) //checks if enemy is on level 1
    {
        if(enemy.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1")) return true;

        if(enemy.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2")) return false;

        else return false;
    }

    public static bool ObjectIsInRealWorld(GameObject objectBeingChecked) //checks if enemy is on level 1
    {
        if(objectBeingChecked.gameObject.layer == LayerMask.NameToLayer("Environement Layer 1")) return true;

        if(objectBeingChecked.gameObject.layer == LayerMask.NameToLayer("Environement Layer 2")) return false;

        if(objectBeingChecked.gameObject.layer == LayerMask.NameToLayer("Hook Layer 1")) return true;

        if(objectBeingChecked.gameObject.layer == LayerMask.NameToLayer("Hook Layer 2")) return false;

        else return false;
    }

    private void CompareEnemyToPlayerLayer(List<GameObject> EnemyEntities) //sends message to each enemy to notify them if they share the same layer as the player
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        foreach (GameObject Enemy in EnemyEntities)
        {
            if(Enemy.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1")
            && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 1"))
            {
                PriestClass.isOnMyLayer = true;
                MonsterClass.isOnMyLayer = true;
            }
            
            else if(Enemy.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1")
            && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 2"))
            {
                PriestClass.isOnMyLayer = false;
                MonsterClass.isOnMyLayer = false;
            }

            else if(Enemy.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2")
            && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 2"))
            {
                PriestClass.isOnMyLayer = true;
                MonsterClass.isOnMyLayer = true;
            }

            else if(Enemy.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2")
            && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 1"))
            {
                PriestClass.isOnMyLayer = false;
                MonsterClass.isOnMyLayer = false;
            }
        }
    }
}
