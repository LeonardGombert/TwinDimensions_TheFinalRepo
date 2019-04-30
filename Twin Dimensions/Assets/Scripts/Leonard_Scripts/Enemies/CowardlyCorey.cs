using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowardlyCorey : PriestClass
{
    [SerializeField]
    public List<Animation> animations = new List<Animation>();
    [SerializeField]
    public Animation[] animations2;

    public Animation anim3;

    // Start is called before the first frame update
    public override void Awake()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        //SwitchWorldsBitch();
    }

    private void SwitchWorldsBitch()
    {
        if (base.MyLayerChecker()) //if on my layer
        {
            anim.SetBool("isOnMyLayer", true);            
            Switch();
        }

        if (!base.MyLayerChecker()) //if not on my layer
        {
            anim.SetBool("isOnMyLayer", false);     
        }        
    }

    private IEnumerator WaitForAnimation (Animation animation)
    {
        do yield return null;
        
        while (animation.isPlaying);
    }

    private void Switch()
    {        
        if (LayerManager.EnemyIsInRealWorld(this.gameObject)) 
        LayerManager.LayerSwitchManager(this.gameObject, "Enemy Layer 2");

        else if (!LayerManager.EnemyIsInRealWorld(this.gameObject)) 
        LayerManager.LayerSwitchManager(this.gameObject, "Enemy Layer 1");
    }
}
