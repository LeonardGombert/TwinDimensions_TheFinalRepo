using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class ActivationPriestController : MonsterClass
{
    [SerializeField]List<GameObject> hitEnemies = new List<GameObject>();
    private GameObject hitEnemy;

    // Start is called before the first frame update
    public override void Awake()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {

    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Projectile")
        {
            hitEnemies.Add(collision.gameObject);

            foreach (GameObject Enemy in hitEnemies) Enemy.gameObject.SendMessage("SwitchedByPriest", true);
        }

        if(collision.tag == "Elephant")
        {
            dontDestroyManager = GameObject.FindGameObjectWithTag("DontDestroyManager");
            Debug.Log("The Elephant hit " + gameObject.name);
            dontDestroyManager.gameObject.SendMessage("WasKilled", this.gameObject);
            GenerateSand();
            Destroy(gameObject);
        }
    }
    
    public override void OnDestroy()
    {
        foreach (GameObject Enemy in hitEnemies) Enemy.gameObject.SendMessage("SwitchedByPriest", false);
    }
}