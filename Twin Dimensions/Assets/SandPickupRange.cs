using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandPickupRange : MonoBehaviour
{
    [SerializeField] Animator anim;
    GameObject manager; 

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
    }

    void Update()
    {
        if(LayerManager.PlayerIsInRealWorld()) this.gameObject.layer = LayerMask.NameToLayer("Sand Layer 1");
        if(!LayerManager.PlayerIsInRealWorld()) this.gameObject.layer = LayerMask.NameToLayer("Sand Layer 2");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Sand")
        {
            anim.SetTrigger("gainedSand");
            manager.gameObject.SendMessage("AddNewSandShard", 1);
            Destroy(collider.gameObject);
        }        
    }
}
