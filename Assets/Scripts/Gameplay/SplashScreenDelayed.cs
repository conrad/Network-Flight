using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreenDelayed : MonoBehaviour {
    public float delayTime = 4.0f;



    IEnumerator Start() 
    {
        // Use IEnumerator when you're using yield statements.
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene("Menu");
    }
}
