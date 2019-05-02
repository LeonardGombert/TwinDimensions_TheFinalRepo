using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class CowardlyCorey : MonsterClass
{
    // Start is called before the first frame update
    public override void Awake()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        CheckPlayerLayer();
    }

    void CheckPlayerLayer()
    {
        if(this.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1") && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 1")) Switch();
        if(this.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2") && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 2")) Switch();
        else return;
    }

    private void Switch()
    {        
        if (LayerManager.EnemyIsInRealWorld(this.gameObject)) 
        LayerManager.LayerSwitchManager(this.gameObject, "Enemy Layer 2");

        else if (!LayerManager.EnemyIsInRealWorld(this.gameObject)) 
        LayerManager.LayerSwitchManager(this.gameObject, "Enemy Layer 1");
    }
}
