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
    GameObject dontDestroyManager;

    // Start is called before the first frame update
    public override void Awake ()
    {
        dontDestroyManager = GameObject.FindGameObjectWithTag("DontDestroyManager");
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {        
        if(collider.tag == "Player")
        {
            Debug.Log("I hit the Player");
            PlayerController.playerIsDead = true;
        }

        if(collider.tag == "Elephant" || collider.gameObject.tag == "Statue")
        {
            Debug.Log("The Elephant hit " + gameObject.name);
            //dontDestroyManager.gameObject.SendMessage("WasKilled", this.gameObject);
            anim.SetBool("isActive", true);
            Animator[] fireAnim = GetComponentsInChildren<Animator>();

            foreach(Animator anim in fireAnim)
            {
                anim.SetBool("FireFade", true);
            }

            GenerateSand();
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "overLayering") sr.sortingLayerName = "Enemy_underProps";
        if(collider.tag == "underLayering") sr.sortingLayerName = "Enemy_overProps";
    }
}