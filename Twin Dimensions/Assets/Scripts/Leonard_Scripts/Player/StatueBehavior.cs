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
    Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PunchStatue();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("I've hit the Player");
        }
    }   

    private void PunchStatue()
    {
        if(StatueManager.isPunchingStatue == true)
        {
            kickDirection = (transform.position - player.position).normalized;

            if(PlayerInputManager.instance.GetKeyDown("kickStatue"))
            {
                rb.AddForce(kickDirection * StatueManager.statueKickSpeed);

                anim.SetFloat("xDirection", kickDirection.x);
                anim.SetFloat("yDirection", kickDirection.y);
                anim.SetTrigger("isPunching");

                StatueManager.isPunchingStatue = false;
            }
        }
    }
}
