using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;



public class GameOver : MonoBehaviour 
{
    public GameObject countdownObject;
    public GameObject gameOverObject;
    public string winner;

//    private GameObject playerView;
    private float totalCountDownTime = 10.0f;
    private float countDownTime;
    private float gameOverTime;
    private TextMesh gameOverTextMesh;
    private TextMesh countdownTextMesh;



    void Start()
    {
        gameOverTime = Time.fixedTime;
        countDownTime = totalCountDownTime;

        gameOverTextMesh = gameOverObject.GetComponent<TextMesh>();
        countdownTextMesh = countdownObject.GetComponent<TextMesh>();

        SetGameOverMesh();
        SetCountdownMesh();
    }



    public void SetWinner(int winnerNum)
    {
        winner = winnerNum.ToString();
    }



    void Update() 
    {
        SetCountdownMesh();

        if (countDownTime <= 0) {
            GoToTransitionToMenu();
        }
    }



    void SetGameOverMesh()
    {
        if (winner != "0") {
            gameOverTextMesh.text = "Player " + winner + " wins!";
        } else {
            gameOverTextMesh.text = "Game Over";
        }
    }



    void SetCountdownMesh()
    {
        countDownTime = totalCountDownTime - (Time.fixedTime - gameOverTime);
        countDownTime = countDownTime > 0 ? countDownTime : 0;

        countdownTextMesh.text = Math.Ceiling(countDownTime).ToString();
    }



    void GoToTransitionToMenu()
    {
        SceneManager.LoadScene("TransitionToMenu");
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
