using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyTurret : MonoBehaviour
{
    public float offset;

    public GameObject projectile;

    public List<Transform> shotPoints = new List<Transform>();
    private float timeBtwShots;
    public float startTimeBtwShots;
    Vector3 direction;
    float bulletForce;

    private void Update()
    {
        // Handles the weapon rotation
        Shoot();
    }

    private void Shoot()
    {  
        foreach (Transform shotPosition in shotPoints)
        {
            if (timeBtwShots <= 0)
            {
                Instantiate(projectile, shotPosition.position, transform.rotation);
                timeBtwShots = startTimeBtwShots;
            }

            else 
            {
                timeBtwShots -= Time.deltaTime;
            }
        }
    }
}
