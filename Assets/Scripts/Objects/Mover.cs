using System.Collections;
using UnityEngine;


/**
 * This class moves an object straight forward always, 
 * as opposed to have the object follow a target, which happen in ObjectMover.
 */ 
public class Mover : MonoBehaviour 
{
	public float speed;
	Rigidbody rigidbody;



	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		rigidbody.velocity = transform.forward * speed;
	}
}
