using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector3 mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition = ExtensionMethods.getFlooredWorldPosition(mousePosition);
    
        if(LayerManager.EnemyIsInRealWorld(this.gameObject)) 
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, mousePosition, 50f, LayerMask.GetMask("Enemy Layer 1"));
            
            if(hit.collider)
            {
                hit.collider.gameObject.SendMessage("ActivateTriggerBehavior");
                Debug.Log("I've hit " + hit.collider.gameObject.name);
                isActive = false;
            }
        }

        if(!LayerManager.EnemyIsInRealWorld(this.gameObject)) 
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, mousePosition, 50f, LayerMask.GetMask("Enemy Layer 2"));
            
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
