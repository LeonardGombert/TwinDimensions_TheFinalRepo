using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class MonsterClass : SerializedMonoBehaviour
{
    [FoldoutGroup("World Switching")][SerializeField]
    public SpriteRenderer spiritWorldVisuals;
    [FoldoutGroup("World Switching")][SerializeField]
    public SpriteRenderer realWorldVisuals;        
    [FoldoutGroup("Sprite Switching")][SerializeField]
    public List<Sprite> spriteList = new List<Sprite>();

    [FoldoutGroup("Visual Component References")]
    public SpriteRenderer sr;
    [FoldoutGroup("Visual Component References")]
    public Animator anim;

    [HideInInspector]
    public GameObject player;

    public static bool isBeingSwitchedByPriest;
    public static bool isBeingCharged;
    public static bool isBeingTeleported;

    public static bool isInAltMode = false; 
    public static bool isInActiveMode = false;

    public static bool isOnMyLayer = false;

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
        if(isActivatedByTurret()) TriggerBehavior();
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

    public virtual void TriggerBehavior(){}

    private bool isActivatedByTurret(bool messageListener = false)
    {
        if(messageListener == true) return true;
        else return false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("I've hit the Player");
        }

        if(collision.tag == "ActivationPriest")
        {
            Debug.Log("The Priest has activated me" + this.gameObject.name);
            isBeingSwitchedByPriest = true;
        }
        
        if(collision.tag == "Elephant")
        {
            Debug.Log("The Elephant hit " + gameObject.name);
            Destroy(gameObject);
        }

        if(collision.tag == "Trap")
        {
            Destroy(gameObject);
        }
    }
}