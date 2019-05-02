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
        Vector3 mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = ExtensionMethods.getFlooredWorldPosition(mousePosition);
    
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, mousePosition);

        if(hit.collider)
        {
            hit.collider.gameObject.SendMessage("isActivatedByTurret", true);
            Debug.Log("Lulz 4 dayz, I have touched " + hit.collider.gameObject.name);
        }
    }
}
