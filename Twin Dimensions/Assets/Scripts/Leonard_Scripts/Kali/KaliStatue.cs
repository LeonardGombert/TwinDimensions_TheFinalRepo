using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class KaliStatue : MonoBehaviour
{
    [SerializeField] GameObject Kali;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerInputManager.instance.GetKeyDown("interactionKey")) Kali.gameObject.SendMessage("DealDamage", "Elephant");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Elephant") Kali.gameObject.SendMessage("DealDamage", "Elephant");
        if(collider.tag == "Elephant") Kali.gameObject.SendMessage("DealDamage", "Elephant");
        if(collider.tag == "Elephant") Kali.gameObject.SendMessage("DealDamage", "Elephant");
    }
}
