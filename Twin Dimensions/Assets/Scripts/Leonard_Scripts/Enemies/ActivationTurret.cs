using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Editor;

public class ActivationTurret : MonoBehaviour
{
    bool isActive = false;
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
        Debug.Log("I'm shooting");

        Vector3 mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
        if(LayerManager.EnemyIsInRealWorld(this.gameObject)) 
        {
            RaycastHit2D hit = Physics2D.Linecast(this.transform.position, mousePosition, LayerMask.GetMask("Enemy Layer 1") << LayerMask.GetMask("World Obstacle Detection 1"));
            Debug.DrawLine(this.transform.position, mousePosition, Color.white, 80f); 
            if(hit.collider)
            {
                hit.collider.gameObject.SendMessage("ActivateTriggerBehavior");
                Debug.Log("I've hit " + hit.collider.gameObject.name);
                isActive = false;
            }
        }

        if(!LayerManager.EnemyIsInRealWorld(this.gameObject)) 
        {
            RaycastHit2D hit = Physics2D.Linecast(this.transform.position, mousePosition, LayerMask.GetMask("Enemy Layer 2") << LayerMask.GetMask("World Obstacle Detection 2"));
            Debug.DrawLine(this.transform.position, mousePosition, Color.green, 80f);            
            
            if(hit.collider)
            {
                hit.collider.gameObject.SendMessage("ActivateTriggerBehavior");
                Debug.Log("I've hit " + hit.collider.gameObject.name);
                isActive = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player") isActive = true;
    }
}
