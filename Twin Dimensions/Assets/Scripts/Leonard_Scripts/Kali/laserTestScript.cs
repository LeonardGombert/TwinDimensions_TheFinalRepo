using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserTestScript : MonoBehaviour
{
    public GameObject player;
    Transform playerPosition;
    public LineRenderer lineRenderer;
    public Transform targetPosition;
    RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = true; 
        GetPlayerLocation();
    }

    // Update is called once per frame
    void Update()
    {
        LaserEyeBeam();
    }

    void GetPlayerLocation()
    {
        targetPosition.transform.position = player.transform.position;
        Debug.Log(targetPosition);
    }

    void LaserEyeBeam()
    {
        hit = Physics2D.Linecast(transform.position, targetPosition.position);

        if(hit.collider){
            if(hit.collider.tag == "Obstacle")
            {
                targetPosition.position = new Vector3(hit.point.x, hit.point.y);
            }}

        Debug.DrawLine(transform.position, targetPosition.position, Color.white);

        lineRenderer.SetPosition(0, transform.position); //defines 1st ("start") point
        lineRenderer.SetPosition(1, targetPosition.transform.position); //defines 2nd (or "end") point
    }
}
