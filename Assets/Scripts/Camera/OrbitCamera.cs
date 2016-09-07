using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {

    public GameObject target;
    public float speed = 4;



    void Update () {
        transform.LookAt(target.transform); 

        transform.position += transform.right * speed * Time.deltaTime;
	}
}
