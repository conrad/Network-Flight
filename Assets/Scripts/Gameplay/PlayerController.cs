using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Photon.MonoBehaviour 
{
    public GameObject leftEye;
    public GameObject avatar;
    public GameObject outOfBoundsView;
    public int playerNumber;

    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;
    private Transform playerLocal;
    private Vector3 position;

    private GameConfig GameConfig;
    private Rigidbody rb;
    private bool isMoving = true;
    private Vector3 axis;    
    private int score = 0;
    private ScoringSystem scoringScript;
    private Score scoresScript;       // http://answers.unity3d.com/questions/7555/how-do-i-call-a-function-in-another-gameobjects-sc.html
    private float forwardSpeed;    
    private float rotationSpeed;
    private AudioSource audio;
    private bool isAlive = true;
    private float lerpSmoothing = 5f;
    private bool startPhotonIsMineCalled = false;



	void Start () {  
        GameConfig    = GameConfig.Instance();
        playerLocal   = this.transform.Find("GvrMain/Head/Main Camera/Main Camera Left");
        rotationSpeed = GameConfig.playerRotationSpeed;
        forwardSpeed  = GameConfig.playerForwardSpeed;
        audio         = GetComponent<AudioSource>();
           

        if (photonView.isMine || GameConfig.isSoloGame)  {
            startPhotonIsMineCalled = true;
            rb = GetComponent<Rigidbody>();
            forwardSpeed = GameConfig.playerForwardSpeed;

            avatar.SetActive(false);

            GvrViewer viewer = GetComponentInChildren<GvrViewer>();
            viewer.enabled = true;
            Debug.Log("viewer enabled: " + viewer);

            GvrEye[] eyes = GetComponentsInChildren<GvrEye>(true);
            foreach (GvrEye eye in eyes)
                eye.enabled = true;

            GvrAudioListener gvrListener = GetComponentInChildren<GvrAudioListener>();
            if (gvrListener) {
                gvrListener.enabled = true;
                Debug.Log("listener enabled: " + gvrListener);
            }

            Camera[] cameras = GetComponentsInChildren<Camera>(true);
            foreach (Camera camera in cameras) 
                camera.enabled = true;
        
            GameObject scoring = GameObject.FindWithTag("Scoring System");
            scoringScript = scoring.GetComponent<ScoringSystem>();
            scoringScript.AddPlayer(playerNumber);
//        } else {
//            StartCoroutine("LerpPlayerPosition");
        }
    }



    void Update () 
    {   
        if (photonView.isMine || GameConfig.isSoloGame) {
            if (Input.touchCount > 1 && isMoving) {
                isMoving = false;
                GameObject modal = GameObject.Find("Canvas/MenuModal");
                modal.SetActive(true);
            }

            if (isMoving) {
                forwardSpeed = GameConfig.playerForwardSpeed;
            } else {
                forwardSpeed = 0;
            }

            FlightMode();  
        } else {
            transform.position = Vector3.Lerp (transform.position, realPosition, 0.1f);
            realPosition       = transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
            realRotation       = transform.rotation;
        }
	}



    void FlightMode () 
    {    
        Vector3 nextAxis = GetRotation(leftEye);
        RotatePlayer(nextAxis);
        MovePlayer();
    }



    Vector3 GetRotation(GameObject gameObject)
    {
        float rotationX = gameObject.transform.localRotation.x / 2;        
        float rotationY = gameObject.transform.localRotation.y / 2;        
        float rotationZ = gameObject.transform.localRotation.z;                

        return new Vector3(rotationX, rotationY, rotationZ);
    }



    void RotatePlayer(Vector3 nextAxis)
    {
        transform.Rotate(nextAxis * Time.deltaTime * rotationSpeed);    // Use this methodology to rotate in order to turn at the same rate even if frames dropped
        avatar.transform.rotation = leftEye.transform.rotation;     // Rotate player's avatar
    }



    void MovePlayer()
    {
        rb.velocity = leftEye.transform.forward * forwardSpeed;    //  position = rb.velocity;
    }



    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /* 
         * You must have the order of writing be the same order as 
         * that for reading to keep things straight.
         */
        if (stream.isWriting) {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        } else {
            // Make sure to type cast.
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }



    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Pick Up")) 
        {
            audio.Play();
            other.gameObject.SetActive(false);
            score += 1;
            scoringScript.SetScore(playerNumber, score);
        }

        if (other.gameObject.CompareTag("Wall")) {
            outOfBoundsView.SetActive(true);
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall")) {
            outOfBoundsView.SetActive(false);
        }
    }



    // Lerping is like Tweening. It smooths movements out.
//    IEnumerator LerpPlayerPosition()         // Making this a co-routine avoids hogging resources.
//    {
//        while(isAlive) {
//            transform.position = Vector3.Lerp(  
//                transform.position,
//                position,
//                Time.deltaTime * lerpSmoothing
//            );
//            transform.rotation = Quaternion.Lerp(
//                transform.rotation,
//                leftEye.transform.rotation,
//                Time.deltaTime * lerpSmoothing
//            );
//
//            yield return null;
//        }
//    }
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