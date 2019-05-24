using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : MonoBehaviour
{
    public bool EnemyShooter;
    public bool Spike;
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (col.gameObject.tag.Equals ("ActivationPriest") && EnemyShooter == true)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (col.gameObject.tag.Equals("Projectile") && EnemyShooter == true)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (col.gameObject.tag.Equals("Obstacle") && EnemyShooter == true)
        {
            Destroy(gameObject);
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Elephant") && EnemyShooter == true )
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag.Equals("Elephant") && Spike == true)
        {
            Destroy(collision.gameObject);
        
        }


    }
}
