using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kaliDamageGong : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Elephant")
        {
            if(KaliBossAI.oneInPosition == true)
            {
                KaliBossAI.twoInPosition = true;
                Debug.Log("Ready to fire, cap'n");
            }

            else if(!KaliBossAI.oneInPosition)
            {
                KaliBossAI.oneInPosition = true;
                Debug.Log("Canon one loaded");
            }
        }
    }
}
