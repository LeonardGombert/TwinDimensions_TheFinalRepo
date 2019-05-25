using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class Firebreather : MonsterClass
{
    #region //AudioClip
    [FoldoutGroup("Firebreather SFX")] [SerializeField] AudioClip FireIdle;
    [FoldoutGroup("Firebreather SFX")] [SerializeField] AudioClip Embrasement;
    [FoldoutGroup("Firebreather SFX")] [SerializeField] AudioClip FirebreatherDeath;
    #endregion

    [SerializeField] GameObject parent;

    // Start is called before the first frame update
    public override void Awake ()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        
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

        if(collision.tag == "Elephant")
        {
            dontDestroyManager = GameObject.FindGameObjectWithTag("DontDestroyManager");
            Debug.Log("The Elephant hit " + gameObject.name);
            dontDestroyManager.gameObject.SendMessage("WasKilled", this.gameObject);
            anim.SetBool("isActive", true);
            GenerateSand();
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "Statue")
        {
            //Instantiate(Fireball)
            base.anim.SetBool("isActive", true);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "overLayering") sr.sortingLayerName = "Enemy_underProps";
        if(collider.tag == "underLayering") sr.sortingLayerName = "Enemy_overProps";
    }

    public override void OnDestroy()
    {
        Destroy(parent);
    }
}