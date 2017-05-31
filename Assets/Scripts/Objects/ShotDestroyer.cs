using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDestroyer : MonoBehaviour 
{
	public float lifetime;
	public GameObject explosion;



	void Start () 
	{
		Destroy(gameObject, lifetime);
	}



	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			// TODO: Trigger hit feedback for both players and reduce hit player's health.
			Instantiate(explosion, other.transform.position, other.transform.rotation);
		}

		Destroy(gameObject);
	}
}
