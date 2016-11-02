using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour {

    void Update () {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) {
            Debug.Log("clicked screen");
            SceneManager.LoadScene("Menu");
        }
	}
}
