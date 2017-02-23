using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour 
{
    public GameObject target;
    public float speed = 4;
	public float smoothing = 5; 



    void FixedUpdate () 
	{
        transform.LookAt(target.transform); 

		Vector3 nextPos = transform.position + transform.right * speed * Time.deltaTime;

		transform.position = Vector3.Lerp (transform.position, nextPos, smoothing * Time.deltaTime);
	}
}
