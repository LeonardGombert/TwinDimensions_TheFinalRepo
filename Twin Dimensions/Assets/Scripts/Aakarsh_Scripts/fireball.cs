using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.tag.Equals ("ActivationPriest"))
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }

        if (col.gameObject.tag.Equals("Projectile"))
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }

        if (col.gameObject.tag.Equals("Obstacle"))
        {
            
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Elephant"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
