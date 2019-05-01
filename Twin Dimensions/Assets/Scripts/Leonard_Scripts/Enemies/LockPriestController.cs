using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class LockPriestController : MonoBehaviour
{
    public static bool isOnMyLayer = false;

    [FoldoutGroup("References")][SerializeField]
    public GameObject player;
    
    public SpriteRenderer sr;
    public Animator anim;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public void Update()
    {
        CheckPlayerLayer();
    }

    void CheckPlayerLayer()
    {
        if(this.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 1") 
        && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 1")) Teleportation.isOnLockedLayer = true; //can switch = false

        if(this.gameObject.layer == LayerMask.NameToLayer("Enemy Layer 2") 
        && player.gameObject.layer == LayerMask.NameToLayer("Player Layer 2")) Teleportation.isOnLockedLayer = true;  //can switch = false
        else return;
    }

}