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

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Elephant")
        {
            //Instantiate(Fireball)
            base.anim.SetBool("isActive", true);
            base.GenerateSand();
        }

        if(collider.gameObject.tag == "Statue")
        {
            //Instantiate(Fireball)
            base.anim.SetBool("isActive", true);
        }
    }

    public override void OnDestroy()
    {
        Destroy(parent);
    }
}