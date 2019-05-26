using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectile;
    [Header("1=up,2=down,3=right,4=left")]
    public int direction;
    public float speed;
    public float attackdelay;
    public float lastAttacktime;

    private Vector2 dir;
   
    void Start()
    {
        
    }

    
    void Update()
    {
        switch (direction)
        {
            case 1:
                {
                    dir =new Vector2(0,speed);
                }
                break;
            case 2:
                {
                    dir = new Vector2(0, -speed);
                }
                break;
            case 3:
                {
                    dir = new Vector2(speed,0);
                }
                break;
            case 4:
                {
                    dir = new Vector2(-speed,0);
                }
                break;
            default:
                {
                    dir = new Vector2(0, speed);
                }
                break;
        }
        if (Time.time > attackdelay + lastAttacktime)
        {
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity=dir;
            lastAttacktime = Time.time;
        }
    }
}
