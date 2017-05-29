using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringController : MonoBehaviour 
{
	public GameObject shot;
	public Transform rightGun;
	public Transform leftGun;
	public float firingInterval = 0.5f;

	private GameConfig gameConfig;
	private bool isFiring = false;
	private float firingDistance = 300f;
	private int shotCount;
	private bool shouldShootOnRight = false;


	void Start()
	{
		gameConfig = GameConfig.Instance();
		StartCoroutine("ShootOnIntervalWhileFiring");
	}


	void Update () 
	{
		RaycastHit hit;
		bool didHit = false;

		if (Physics.Raycast(transform.position, transform.forward, out hit, firingDistance)) 
		{
			if (hit.collider.tag == "Pick Up") {
//			if (hit.collider.tag == "Player") {
				isFiring = true;
				didHit = true;
				shotCount = 5;
			}
		}

		if (!didHit && shotCount > 0) {
			shotCount--;
		}

		if (shotCount <= 0) {
			isFiring = false;
		}
	}



	IEnumerator ShootOnIntervalWhileFiring()
	{
		while (true) 
		{
			if (isFiring) {
				FireShot(transform);
			}

			yield return new WaitForSeconds(firingInterval);
		}
	}



	void FireShot(Transform playerTransform)
	{
		if (gameConfig.isSoloGame) {
			// Have this work with networking if in multiplayer.
			if (shouldShootOnRight) {
				Instantiate(shot, rightGun.position, playerTransform.rotation);
			} else {
				Instantiate(shot, leftGun.position, playerTransform.rotation);
			}
		} else {
			if (shouldShootOnRight) {
				PhotonNetwork.Instantiate(shot.name, rightGun.position, playerTransform.rotation, 1);
			} else {
				PhotonNetwork.Instantiate(shot.name, leftGun.position, playerTransform.rotation, 1);
			}
		}
			
		shouldShootOnRight = !shouldShootOnRight;
	}
}
