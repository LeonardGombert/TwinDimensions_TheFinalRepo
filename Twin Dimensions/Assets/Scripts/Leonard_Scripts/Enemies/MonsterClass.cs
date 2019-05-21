using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class MonsterClass : SerializedMonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer spiritWorldVisuals;
    [HideInInspector]
    public SpriteRenderer realWorldVisuals;        
    [HideInInspector]
    public List<Sprite> spriteList = new List<Sprite>();
    [HideInInspector]
    public SpriteRenderer sr;
    [HideInInspector]
    public Animator anim;

    private Vector3 playerDirectionForClass;

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public GameObject gameMaster;

    bool isBeingSwitchedByPriest;
    public static bool isBeingCharged;
    public static bool isBeingTeleported;

    public static bool isInAltMode = false; 
    public static bool isInActiveMode = false;

    public static bool isOnMyLayer = false;

    [FoldoutGroup("Sand")][SerializeField] 
    GameObject sandToDrop;
    
    int amountOfSandToDrop;
    ScoreSystem scoreSystem;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(isActivatedByTurret()) ActivateTriggerBehavior();
    }

    public void MyLayerChecker(bool isOnSameLayer)
    {
        if(isOnSameLayer == true) Debug.Log(this.gameObject.name + " and I are on the same layer");
        if(isOnSameLayer == false) Debug.Log(this.gameObject.name + "and I ain't sharing shit, son");
    }

    public void CheckIfBeingTeleported()
    {
        if(isBeingTeleported == true)
        {
            if (LayerManager.EnemyIsInRealWorld(this.gameObject))
            {
                LayerManager.LayerSwitchManager(gameObject, "Enemy Layer 2");
            }

            else if (!LayerManager.EnemyIsInRealWorld(this.gameObject))
            {
                LayerManager.LayerSwitchManager(gameObject, "Enemy Layer 1");
            }
        }
        isBeingTeleported = false;
    }

    public void CheckBehaviorMode()
    {
        if(isBeingSwitchedByPriest == false)
        {
            if (LayerManager.EnemyIsInRealWorld(this.gameObject))
            {
                isInAltMode = true; 
                isInActiveMode = false;
                sr.sprite = spriteList[0];
                anim.enabled = true;
            }
            
            if (!LayerManager.EnemyIsInRealWorld(this.gameObject))
            {
                isInAltMode = false;
                isInActiveMode = true;
                sr.sprite = spriteList[1];
                anim.enabled = true;
            }          
        }

        if(isBeingSwitchedByPriest == true)
        {
            if (LayerManager.EnemyIsInRealWorld(this.gameObject))
            {
                isInAltMode = false;
                isInActiveMode = true;
                sr.sprite = spriteList[1];
                anim.enabled = true;
            }
            
            if (!LayerManager.EnemyIsInRealWorld(this.gameObject))
            {
                isInAltMode = true; 
                isInActiveMode = false;
                sr.sprite = spriteList[0];
                anim.enabled = true;
            }
        }        
    }

    public virtual void ActivateTriggerBehavior(){}
    public virtual void MonitorSFX(){}

    private bool isActivatedByTurret(bool messageListener = false)
    {
        if(messageListener == true) return true;
        else return false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("I hit the Player");
            PlayerController.playerIsDead = true;
        }

        if(collision.tag == "Statue")
        {
            Destroy(collision.gameObject);
        }

        if(collision.tag == "ActivationPriest")
        {
            Debug.Log("The Priest has activated " + this.gameObject.name);
            isBeingSwitchedByPriest = true;
        }

        if(collision.tag == "Firebreather")        
        {
            Debug.Log("The Firebreather hit " + gameObject.name);            
            GenerateSand(amountOfSandToDrop);
            scoreSystem.gameObject.SendMessage("WasKilled", this.gameObject);
            Destroy(gameObject);
        }

        if(collision.tag == "Elephant")
        {
            Debug.Log("The Elephant hit " + gameObject.name);
            GenerateSand(amountOfSandToDrop);
            Destroy(gameObject);
        }

        if(collision.tag == "Trap")
        {
            Debug.Log("The Elephant hit " + collision.gameObject.name);
            GenerateSand(amountOfSandToDrop);
            Destroy(gameObject);
        }
    }

    void DropSand(int sandAmount)
    {
        amountOfSandToDrop = sandAmount;
    }

    void GenerateSand(int amountOfSandToDrop)
    {
        for (int i = 0; i < amountOfSandToDrop; ++i)
        {
            Instantiate(sandToDrop, transform.position, Quaternion.identity);
        }
    }

    bool SwitchedByPriest(bool isBeingSwitched)
    {
        if(isBeingSwitched == true) isBeingSwitched = true;
        if(isBeingSwitched == false) isBeingSwitched = false;

        return isBeingSwitched;
    }
}