﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Photon.MonoBehaviour 
{
    public GameObject leftEye;
    public float forwardSpeed;    
    public float rotationSpeed = 300f; 
    public GameObject avatar;
    public int playerNumber;
    public Transform playerLocal;

    GameConfig GameConfig;
    Rigidbody rb;
    private bool isMoving = true;
    private Vector3 axis;    
    private int score = 0;
    private ScoringSystem scoringScript;
    private Score scoresScript;       // http://answers.unity3d.com/questions/7555/how-do-i-call-a-function-in-another-gameobjects-sc.html

//    bool isAlive = true;
//    float lerpSmoothing = 5f;

    bool startPhotonIsMineCalled = false;
    bool calledLogged = false;


	void Start () {
        Debug.Log("Starting player controller");
        GameConfig = GameConfig.Instance();

        if (photonView.isMine || GameConfig.isSoloGame)  {
            Debug.Log("Setting up my player in PlayerController");

            startPhotonIsMineCalled = true;
            rb = GetComponent<Rigidbody>();
            forwardSpeed = GameConfig.playerSpeed;
            playerLocal = this.transform.Find("GvrMain/Head/Main Camera/Main Camera Left");

            this.transform.SetParent(playerLocal);
            this.transform.localPosition = Vector3.zero;

            if (GameConfig.isSoloGame) {
                avatar.SetActive(false);    // TODO: Fix avatar positioning not to be in way in solo game.
            }

            GvrViewer viewer = GetComponentInChildren<GvrViewer>();
            viewer.enabled = true;

            GvrEye[] eyes = GetComponentsInChildren<GvrEye>(true);
            foreach (GvrEye eye in eyes)
                eye.enabled = true;

            GvrAudioListener listener = GetComponentInChildren<GvrAudioListener>();
            if (listener) {
                listener.enabled = true;
            }

            Camera[] cameras = GetComponentsInChildren<Camera>(true);
            foreach (Camera camera in cameras) 
                camera.enabled = true;
        
            GameObject scoring = GameObject.FindWithTag("Scoring System");
            scoringScript = scoring.GetComponent<ScoringSystem>();
            scoringScript.AddPlayer(playerNumber);
//        } else {
//            StartCoroutine("Alive");      // For lerping
        }
    }



    void Update () 
    {   
        if (photonView.isMine || GameConfig.isSoloGame) {
            if (GvrViewer.Instance.Triggered) { 
                isMoving = !isMoving;
            }

            if (isMoving) {
                forwardSpeed = GameConfig.playerSpeed;
            } else {
                forwardSpeed = 0;
            }

            FlightMode();  
        }

	}



    void FlightMode () 
    {    
        if (!photonView.isMine && !GameConfig.isSoloGame) {
            Debug.Log("This is not my beautiful flight!");        
        }

        //get rotation values for the leftEye        
        float rotationX = leftEye.transform.localRotation.x / 2;        
        float rotationY = leftEye.transform.localRotation.y / 2;        
        float rotationZ = leftEye.transform.localRotation.z;                

        //put them into a vector        
        axis = new Vector3(rotationX, rotationY, rotationZ);                

        // Rotate - Use this methodology in order to turn at the same rate even if frames dropped
        transform.Rotate(axis * Time.deltaTime * rotationSpeed);

        // Move forward
        rb.velocity = leftEye.transform.forward * forwardSpeed;
        RotateAvatar();
    }



    void RotateAvatar()
    {
        avatar.transform.rotation = leftEye.transform.rotation;
    }



    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /* 
         * You must have the order of writing be the same order as 
         * that for reading to keep things straight.
         */
        if (!startPhotonIsMineCalled && !calledLogged) {
            Debug.Log("OnPhotonSerializedView when Start NOT CALLED!!!!!");
        }
        if (stream.isWriting)
        {
//            if (photonView.isMine) {
//                stream.SendNext(this.transform.position);
//                stream.SendNext(this.transform.rotation);
                stream.SendNext(playerLocal.localPosition);
                stream.SendNext(playerLocal.localRotation);
    //            stream.SendNext(score);
//            }
        }
        else
        {
            // Make sure to type cast.
//            networkPlayerPosition = (Vector3)stream.ReceiveNext();
//            networkPlayerRotation = (Quaternion)stream.ReceiveNext();
            avatar.transform.localPosition = (Vector3)stream.ReceiveNext();
            avatar.transform.localRotation = (Quaternion)stream.ReceiveNext();
//            this.transform.position = (Vector3)stream.ReceiveNext();
//            this.transform.rotation = (Quaternion)stream.ReceiveNext();
//            score = (int)stream.ReceiveNext();
        }
    }



    // while alive do this state-machine
    // Make this a co-routine in order to avoid hogging resources.
//    IEnumerator Alive()
//    {
//        while(isAlive) {
//            // Lerping is like Tweening. It smooths movements out.
//            transform.position = Vector3.Lerp(
//                transform.position,
//                position,
//                Time.deltaTime * lerpSmoothing
//            );
//            transform.rotation = Quaternion.Lerp(
//                transform.rotation,
//                rotation,
//                Time.deltaTime * lerpSmoothing
//            );
//
//            yield return null;
//        }
//    }



    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Pick Up")) 
        {
            Debug.Log("OnTriggerEnter collision detected with " + other);
            // TODO: Play a sound here from the location of the pickUp
            other.gameObject.SetActive (false);
            score += 1;
            scoringScript.SetScore(playerNumber, score);
        }
    }
}









//class NetworkPlayer : Photon.MonoBehaviour {
//    public GameObject myCamera;
//    bool isAlive = true;
//    Vector3 position;
//    Quaternion rotation;
//    float lerpSmoothing = 5f;
//
//    void Start() {
//        if (photonView.isMine) {
//            myCamera.SetActive(true);
//            GetComponent<Motor>().enabled = true;
//            GetComponent<Rigidbody>().useGravity = true;
//
//            SwayBar[] sb = GetComponentsInChildren<SwayBar>();
//
//            foreach(SwayBar bar in sb) {
//                bar.enabled = true;
//            }
//        } else {
//            StartCoroutine("Alive");
//        }
//    }
//
//    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
//        // You must have the order of writing be the same order as that for reading
//        // to keep things straight.
//        if (stream.isWriting) {
//            stream.SendNext(transform.position);
//            stream.SendNext(transform.rotation);
//        } else {
//            // } else if (stream.isReading) {
//            // Make sure to type cast.
//            transform.position = (Vector3)stream.ReceiveNext();
//            transform.rotation = (Quaternion)stream.ReceiveNext();
//        }
//    }
//}



// Failed avatar rotation
//        TODO: GET THE ROTATION OF THE AVATAR TO WORK AT 90 DEGREES IN FULL ROTATION
//        THESE IMPLEMENTATIONS DOESN'T FULLY ROTATE...
//        avatar.transform.rotation = new Quaternion(
//            leftEye.transform.rotation.x + 1.0f, 
//            leftEye.transform.rotation.y,
//            leftEye.transform.rotation.z, 
//            1.0f
//        );

//        avatar.transform.rotation = Quaternion.EulerAngles(
//            new Vector3(
//                leftEye.transform.rotation.x + 1.0f, 
//                leftEye.transform.rotation.y,
//                leftEye.transform.rotation.z
//            )
//        );


//        THIS IMPLEMENTATION DOESN'T ROTATE AT ALL...
//        float rotationX = leftEye.transform.localRotation.x / 2;        
//        float rotationY = leftEye.transform.localRotation.y / 2;        
//        float rotationZ = leftEye.transform.localRotation.z;                
//        //put them into a vector        
//        axis = new Vector3(rotationX, rotationY, rotationZ);                
//        // Rotate - Use this methodology in order to turn at the same rate even if frames dropped
//        avatar.transform.Rotate(axis * Time.deltaTime * rotationSpeed);



// update array with index & new value
// update text on score 