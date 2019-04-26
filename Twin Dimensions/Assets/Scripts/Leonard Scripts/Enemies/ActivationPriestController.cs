using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class ActivationPriestController : PriestClass
{
    List<GameObject> hitEnemies = new List<GameObject>();
    private GameObject hitEnemy;

    // Start is called before the first frame update
    public override void Awake()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {

    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        hitEnemies.Add(collider.gameObject);

        foreach (GameObject Enemy in hitEnemies)
        {
            MonsterClass.isBeingSwitchedByPriest = true;
        }
    }
}