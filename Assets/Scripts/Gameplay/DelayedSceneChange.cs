using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DelayedSceneChange : MonoBehaviour {
    public float delayTime = 3.0f;
	public string nextScene = "Menu";


    IEnumerator Start() 
    {
        // Use IEnumerator when you're using yield statements.
        yield return new WaitForSeconds(delayTime);

		SceneManager.LoadScene(nextScene);
    }
}
