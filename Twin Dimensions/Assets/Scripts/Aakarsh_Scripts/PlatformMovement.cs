using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour 
{
		Vector3 posA;
		Vector3 posB;
		Vector3 nexPos;

		[SerializeField] float speed;

		[SerializeField] Transform childTransform = default;
		[SerializeField] Transform transformB = default;

	// Use this for initialization
	void Start () {
		posA = childTransform.localPosition;
		posB = transformB.localPosition;
		nexPos = posB;
	}
	
	// Update is called once per frame
	void Update () {
	
		Move ();
	}
	private void Move ()
	{
		childTransform.localPosition = Vector3.MoveTowards (childTransform.localPosition, nexPos, speed * Time.deltaTime);

		if (Vector3.Distance (childTransform.localPosition, nexPos) <= 0.1) 
		{
			ChangeDestination ();
		}
	}

	private void ChangeDestination()
	{
		nexPos = nexPos != posA ? posA : posB;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
        
		if (other.gameObject.tag == "Player") 
		{
            Debug.Log("Pyaar ek dhoka hai");
            //other.gameObject.layer = 8;
            other.transform.SetParent (childTransform);
		}
	}
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Pyaar ek dhoka hai");
            //other.gameObject.layer = 8;
            other.transform.SetParent(childTransform);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
	{
        Debug.Log("Pyaar ek dhoka hai");
        other.transform.SetParent(null);
	}

}
