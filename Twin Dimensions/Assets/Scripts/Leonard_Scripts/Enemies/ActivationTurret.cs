using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationTurret : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)) Shoot();
    }

    private void Shoot()
    {
        Vector3 mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition = ExtensionMethods.getFlooredWorldPosition(mousePosition);
    
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, mousePosition);

        if(hit.collider.tag == "Elephant")
        {
            hit.collider.gameObject.SendMessage("TriggerBehavior");
            Debug.Log("Lulz 4 dayz, I have touched " + hit.collider.gameObject.name);
        }
    }
}
