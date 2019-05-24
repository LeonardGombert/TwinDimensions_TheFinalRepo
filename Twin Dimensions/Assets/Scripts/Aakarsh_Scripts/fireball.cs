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

    private void OnCollisionEnter2D (Collision2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }

        if (collider.gameObject.tag == "Projectile")
        {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }

        if (collider.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }

        if (collider.gameObject.tag == "Elephant")
        {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}
