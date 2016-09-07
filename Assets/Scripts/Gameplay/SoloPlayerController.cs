using UnityEngine;
using System.Collections;

public class SoloPlayerController : MonoBehaviour {

    public GameObject leftEye;
    Rigidbody rr;
    public float forwardSpeed = 100f;    
    private float rotationSpeed = 300f;        
    private Vector3 axis;
    public int totalPickUps = 10;
    public GameObject pickUp;

	// Use this for initialization
	void Start () {
        rr = GetComponent<Rigidbody>();

        for (var i = 0; i < totalPickUps; i++) {
            Instantiate(pickUp, new Vector3(Random.Range(-450.0f, 450.0f), Random.Range(0, 40), Random.Range(-450.0f, 450.0f)), Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
        FlightMode();        
	}

    void FlightMode () {                
        //get rotation values for the leftEye        
        float rotationX = leftEye.transform.localRotation.x / 2;        
        float rotationY = leftEye.transform.localRotation.y / 2;        
        float rotationZ = leftEye.transform.localRotation.z;                

        //put them into a vector        
        axis = new Vector3 (rotationX, rotationY, rotationZ);                

        // Rotate - Use this methodology in order to turn at the same rate even if frames dropped
        transform.Rotate (axis * Time.deltaTime * rotationSpeed);
        // Move forward
        rr.velocity = leftEye.transform.forward * forwardSpeed;
    }
}
