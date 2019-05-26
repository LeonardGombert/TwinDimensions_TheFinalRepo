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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("ActivationPriest") && EnemyShooter == true)
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }

        if (col.gameObject.tag.Equals("Projectile") && EnemyShooter == true)
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }

        if (col.gameObject.tag.Equals("Obstacle") && EnemyShooter == true)
        {
            Destroy(gameObject);
        }

        if (col.gameObject.tag.Equals("Enemy") && EnemyShooter == true)
        {
            Destroy(col.gameObject);
        }

        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Elephant") && EnemyShooter == true)
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

