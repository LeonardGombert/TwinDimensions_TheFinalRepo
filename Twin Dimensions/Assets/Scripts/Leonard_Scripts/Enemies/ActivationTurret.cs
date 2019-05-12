using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Editor;

public class ActivationTurret : MonoBehaviour
{
    bool isActive = false;    
    Vector3 mousePosition = new Vector3();
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && isActive) Shoot();
    }

    private void Shoot()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
        if(LayerManager.EnemyIsInRealWorld(this.gameObject)) 
        {
            RaycastHit2D hit = Physics2D.Linecast(this.transform.position, mousePosition, LayerMask.GetMask("Enemy Layer 1"));
            Debug.DrawLine(this.transform.position, mousePosition, Color.white, 80f);
            
            if(hit.collider.tag == "Elephant" || hit.collider.tag == "Enemy")
            {
                hit.collider.gameObject.SendMessage("ActivateTriggerBehavior");
                Debug.Log("I've hit " + hit.collider.gameObject.name);
                isActive = false;
            }
        }

        if(!LayerManager.EnemyIsInRealWorld(this.gameObject)) 
        {
            RaycastHit2D hit = Physics2D.Linecast(this.transform.position, mousePosition, LayerMask.GetMask("Enemy Layer 2"));
            Debug.DrawLine(this.transform.position, mousePosition, Color.green, 80f);            
            
            if(hit.collider.tag == "Elephant" || hit.collider.tag == "Enemy")
            {
                hit.collider.gameObject.SendMessage("ActivateTriggerBehavior");
                Debug.Log("I've hit " + hit.collider.gameObject.name);
                isActive = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player" && PlayerInputManager.instance.GetKeyDown("interactionKey"))        
        {isActive = true;
            Debug.Log("I've hit the player");
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player" && PlayerInputManager.instance.GetKeyDown("interactionKey"))        
        {
            isActive = true;
            Debug.Log("I've hit the player");
        }
    }
}
