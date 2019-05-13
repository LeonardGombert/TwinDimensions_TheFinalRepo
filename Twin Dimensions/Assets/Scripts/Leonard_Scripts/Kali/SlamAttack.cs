using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttack : MonoBehaviour
{
    public GameObject Kali;

    // Start is called before the first frame update
    void Start()
    {
        //Kali = GameObject.FindGameObjectWithTag("Kali").GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            Debug.Log("I hit the player");

            Kali.gameObject.SendMessage("WhichSide", this.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            Debug.Log("I hit the player");

            Kali.gameObject.SendMessage("WhichSide", this.gameObject);
        }
    }
}