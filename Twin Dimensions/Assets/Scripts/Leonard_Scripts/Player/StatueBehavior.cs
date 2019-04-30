using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class StatueBehavior : SerializedMonoBehaviour
{
    public Transform player;
    Vector3 currentPositionOnGrid;
    Vector3 kickDirection;
    public Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfKickingStatue();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("I've hit the Player");
        }
    }   

    private void CheckIfKickingStatue()
    {
        if(StatueManager.isKickingStatue == true)
        {
            Debug.Log("Is kicking statue");

            kickDirection = (transform.position - player.position).normalized;

            //kickDirection = ExtensionMethods.RemoveDiagonalsForStatues(kickDirection);

            if(PlayerInputManager.instance.GetKeyDown("kickStatue"))
            {
                Debug.Log("I'm working");
                rb.AddForce(kickDirection * StatueManager.statueKickSpeed);
                StatueManager.isKickingStatue = false;
            }
        }
    }
}
