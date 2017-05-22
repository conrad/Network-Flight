using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringController : MonoBehaviour 
{
//	public GameObject shot;

	private bool isFiring = false;
	private float firingDistance = 100f;
	private int shotCount;


	void Update () 
	{
		RaycastHit hit;
		bool didHit = false;

		if (Physics.Raycast(transform.position, transform.forward, out hit, firingDistance)) 
		{
			if (hit.collider.tag == "Player") {
				isFiring = true;
				didHit = true;
				shotCount = 5;
			}
		}

		if (isFiring) {
			FireShot();
		}

		if (!didHit && shotCount > 0) {
			shotCount--;
		}

		if (shotCount <= 0) {
			isFiring = false;
		}
	}


	void FireShot()
	{
//		Network.Instantiate(shot);
		Debug.Log("pew pew");
	}
}
