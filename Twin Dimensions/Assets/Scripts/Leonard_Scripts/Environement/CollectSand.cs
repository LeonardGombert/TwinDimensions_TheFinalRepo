using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectSand : MonoBehaviour
{
    Animator anim;
    GameObject manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
    }

    // Update is called once per frame
    void Update()
    {
        
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

    void OnTriggerStay2D(Collider2D collider)
    {        
        if(collider.tag == "Sand")
        {
            manager.gameObject.SendMessage("AddNewSandShard", 1);
            Destroy(collider.gameObject);
        }
    }
}
