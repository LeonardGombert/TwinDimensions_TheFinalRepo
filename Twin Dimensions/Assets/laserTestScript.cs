using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserTestScript : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform hitPosition;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, hitPosition.transform.position);

        Debug.DrawRay(transform.position, hitPosition.transform.position, Color.white);

        lineRenderer.SetPosition(0, transform.position); //defines 1st ("start") point
        lineRenderer.SetPosition(1, hitPosition.position); //defines 2nd (or "end") point
    }
}
