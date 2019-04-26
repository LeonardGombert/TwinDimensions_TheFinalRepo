using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyTurret : MonoBehaviour
{
    public Rigidbody2D myBullet;
    public List<Transform> TurretBarrels;

    public Transform parentObject;
    Vector3 direction;

    public int delayTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        foreach (Transform MuzzleEnd in TurretBarrels)
        {
            Instantiate(myBullet, MuzzleEnd.position, Quaternion.identity);

            direction = (MuzzleEnd.position - parentObject.position).normalized;
            myBullet.AddForce(direction * 4, ForceMode2D.Impulse);
            Debug.Log("This is " + direction + "'s direction relative to center");
        }     

        yield return new WaitForSeconds(delayTime);
    }
}
