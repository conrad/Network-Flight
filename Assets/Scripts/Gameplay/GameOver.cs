using UnityEngine;
using System.Collections;
using System.Threading;

public class GameOver : MonoBehaviour 
{
    public GameObject countdownObject;
    public GameObject gameOverObject;
    public string winner;

//    private GameObject playerView;
    private float time = 10.0f; 
    private TextMesh gameOverTextMesh;
    private TextMesh countdownTextMesh;



    void Start()
    {
        gameOverTextMesh = gameOverObject.GetComponent<TextMesh>();
        countdownTextMesh = gameOverObject.GetComponent<TextMesh>();

        countdownTextMesh.text = Mathf.RoundToInt(time).ToString();
        gameOverTextMesh.text = "waiting...";
        Debug.Log("gameOverTextMesh in Start " + gameOverTextMesh);
    }



    public void SetWinner(int winnerNum)
    {
        Debug.Log("setWinner");
        winner = winnerNum.ToString();

        SetGameOverText();
    }

    private void SetGameOverText()
    {
        SetMeshes();
        Debug.Log("gameOverTextMesh in setGameOverText " + gameOverTextMesh);
        gameOverTextMesh.text = "Player " + winner + " Wins!";
    }


    void Update() 
    {
        // Update the number for the countdown:  ceiling    
        time =- Time.deltaTime; 
        countdownTextMesh.text = Mathf.RoundToInt(time).ToString(); 
    }

    void SetMeshes()
    {
        gameOverTextMesh = gameOverObject.GetComponent<TextMesh>();
        countdownTextMesh = gameOverObject.GetComponent<TextMesh>();

        countdownTextMesh.text = Mathf.RoundToInt(time).ToString();
        gameOverTextMesh.text = "waiting...";

    }
}


//    public void SetPlayerViewObject(GameObject playerViewObject) 
//    {
//        playerView = playerViewObject;
//    }
//
//
//
//    public void SetTransform() 
//    {
//        this.transform.parent = playerView.transform;
//        this.transform.localPosition = new Vector3(-4f, 2f, 8f);
//        this.transform.rotation = playerView.transform.rotation;
//    }
