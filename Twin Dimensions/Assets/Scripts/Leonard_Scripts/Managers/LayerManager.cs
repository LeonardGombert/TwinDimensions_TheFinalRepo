using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class LayerManager : SerializedMonoBehaviour
{
    public static LayerManager instance;

    [FoldoutGroup("Enemies in Scene")][SerializeField]
    List <GameObject> fillThisListWithEnemies;
    
    GameObject player;
    

    void Awake()
    {
        if(instance == null)
        {            
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CompareEnemyToPlayerLayer(fillThisListWithEnemies);
    }

    public static LayerMask PlayerRaycastLayerManager()
    {
        GameObject player;
        player = GameObject.FindGameObjectWithTag("Player");
        
        LayerMask currentLayer = player.gameObject.layer;
        LayerMask desiredRayLayer = new LayerMask();

        if(currentLayer == LayerMask.NameToLayer("Player Layer 1"))
        {
            desiredRayLayer = LayerMask.NameToLayer("Ray Layer 1");
        }

        if(currentLayer == LayerMask.NameToLayer("Player Layer 2"))
        {
            LayerMask.NameToLayer("Ray Layer 2");
        }

        return desiredRayLayer;
    }

    public static void LayerSwitchManager(GameObject gameObject, string layerName)
    {
        if (gameObject == null) return;

        Transform[] childList = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in childList)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    public static bool PlayerIsInRealWorld()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player.gameObject.layer == LayerMask.NameToLayer("Player Layer 1")) return true;

        if(player.gameObject.layer == LayerMask.NameToLayer("Player Layer 2")) return false;

        else return false;
    }

    public static bool EnemyIsInRealWorld(GameObject enemy)
    {
        if(enemy.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1")) return true;

        if(enemy.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2")) return false;

        else return false;
    }

    private void CompareEnemyToPlayerLayer(List<GameObject> EnemyEntities)
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
