using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class DestroyableWall : MonoBehaviour
{   
    //Animator anim;

    void Awake()
    {
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
             
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Elephant")
        {
            //collider.SendMessage("HasHitWall");
            WasHitByElephant();
        }
    }

    void WasHitByElephant()
    {
        //anim.Play("destroyWall");
        Destroy(this.gameObject);
        //Unlock wall
    }
}
