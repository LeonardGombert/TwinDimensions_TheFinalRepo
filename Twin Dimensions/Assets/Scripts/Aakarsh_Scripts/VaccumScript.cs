using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaccumScript : MonoBehaviour
{

    public List<GameObject> ItemsToVacuum = new List<GameObject>();
    public float suctionPower = 1;
    public bool isVacuuming;
    Vector3 vacuumDirection;
    public GameObject Target;


    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            {
                vacuumDirection = (Target.transform.position - transform.position).normalized;
                Target.GetComponent<Rigidbody2D>().AddForce(-vacuumDirection * suctionPower);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            {
                vacuumDirection = (Target.transform.position-transform.position).normalized;
                Target.GetComponent<Rigidbody2D>().AddForce(-vacuumDirection * suctionPower);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            {
                Target.GetComponent<Rigidbody2D>().velocity=new Vector2(0,0);
            }
        }
    }
}



