using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaliDoor : MonoBehaviour
{
    Animator anim; 

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player") 
        {
            anim.SetBool("openDoor", true);
            PlayerController.cinematicMoveUp = true;
        }
    }
}
