using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour {
    void Start() {
        // TODO: Set up the instructions & menu so that this isn't necessary.
        PhotonNetwork.Disconnect ();
        GameObject networkManager = GameObject.Find("PhotonNetworkManager");
        Destroy(networkManager);
    }

    void Update () {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("Menu");
        }
	}
}
