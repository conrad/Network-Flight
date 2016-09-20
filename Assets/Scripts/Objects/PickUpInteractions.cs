using UnityEngine;
using System.Collections;

public class PickUpInteractions : MonoBehaviour {

    public AudioClip pickUpSound;

    private AudioSource source;
//    private float volRangeLow = 0.5f;
//    private float volRangeHigh = 1.0f;



	void Awake () {
        source = GetComponent<AudioSource>();
	}
	


    void OnTriggerEnter(Collider other) 
    {
        source.PlayOneShot(pickUpSound, 1f);
    }
}
