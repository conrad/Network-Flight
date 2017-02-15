using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour 
{
	void FixedUpdate () 
	{
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}
}
