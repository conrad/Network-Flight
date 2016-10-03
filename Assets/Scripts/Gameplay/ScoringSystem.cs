using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;



/**
 * A class to track all players and their scores and trigger the view updates based on changes.
 */ 
public class ScoringSystem : Photon.MonoBehaviour 
{
    public GameObject gameOverView;

    private int[] scores = new int[6]; 
    private Dictionary<int, Score> scoreboardScripts = new Dictionary<int, Score>();
    private Dictionary<string, int> highScorerData = new Dictionary<string, int>();
    GameConfig GameConfig;
    private bool isShowGameOverRevealed = false;
    private float startTime;

    void Start() 
    {
        startTime = Time.fixedTime;
        GameConfig = GameConfig.Instance();
        SetInitialHighScorerData();
        highScorerData["totalScored"] = 0;
    }


    /**
     * Check whether game has gone over time, and end if it has.
     */ 
    void Update()
    {
        if (Time.fixedTime > (GameConfig.timeLimit + startTime)) {
            ShowGameOverOnce(highScorerData["player"]);
        }
    }



    /**
     * Update collection of references for precise access to each player's script.
     */ 
    private void UpdateScoreboardScriptCollection() 
    {
        GameObject[] unorderedScoreboards = new GameObject[2];
        unorderedScoreboards = GameObject.FindGameObjectsWithTag("Score");

        foreach (GameObject scoreboard in unorderedScoreboards) {
            Score scoreScript = scoreboard.GetComponent<Score>();
            scoreboardScripts[scoreScript.GetPlayerNumber()] = scoreScript;
        }
    }



    /**
     * Add new score to array of scores and update script references to accommodate new player.
     */ 
    public void AddPlayer(int playerNumber) {
        scores[playerNumber] = 0;
        UpdateScoreboardScriptCollection();
        scoreboardScripts[playerNumber].UpdateScoreView(playerNumber, scores[playerNumber]);
    }



    /**
     * Add score to tracked scores, determine current highest scorer, determine whether game is over.
     */ 
    public void SetScore(int playerNumber, int score) {
        scores[playerNumber] = score;
        highScorerData["totalScored"]++;
        scoreboardScripts[playerNumber].UpdateScoreView(playerNumber, scores[playerNumber]);

        if (score > highScorerData["score"]) {
            highScorerData["score"] = score;
            highScorerData["player"] = playerNumber;
        }

        if (IsGameOver(highScorerData["totalScored"])) {
            ShowGameOver(highScorerData["player"]);
        }
    }



    public int[] GetScores() {
        return scores;
    }



    public int GetScore(int playerNumber) {
        return scores[playerNumber];
    }



    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) {
            stream.SendNext(scores);
        } else {
            int[] newScores = (int[])stream.ReceiveNext();
            UpdateView(newScores);
        }
    }



    /**
     * TODO: Should this be called setScores and just call setScore in a for loop???
     */
    private void UpdateView(int[] newScores) {
        SetInitialHighScorerData();

        for (int i = 1; i < scores.Length; i++) {
            if (scoreboardScripts[i] == null) {
                Debug.Log("Whoa. We don't have a score script for player " + i);      
            }
                
            if (newScores[i] != scores[i]) {
                scores[i] = newScores[i];
                scoreboardScripts[i].UpdateScoreView(i, scores[i]);           // try/catch
            }

            UpdateHighScorerData(i);
            Debug.Log("totalScored: " + highScorerData["totalScored"]);

            if (IsGameOver(highScorerData["totalScored"])) {
                ShowGameOver(highScorerData["player"]);
            }
        }

        Debug.Log("totalScored: " + highScorerData["totalScored"]);
    }



    private void SetInitialHighScorerData() 
    {
        highScorerData["player"] = 0;
        highScorerData["score"] = 0;
    }



    private void UpdateHighScorerData(int playerNum)
    {
        highScorerData["totalScored"] += scores[playerNum];
            
        if (scores[playerNum] > highScorerData["score"]) {
            highScorerData["player"] = playerNum;
            highScorerData["score"] = scores[playerNum];
        }
    }



    private bool IsGameOver(int totalScored) 
    {
        return totalScored >= GameConfig.totalPickUps;
    }

    // Invoke ShowGameOver if it hasn't already been called.
    private void ShowGameOverOnce(int winnerNum)
    {
        if (!isShowGameOverRevealed) {
            ShowGameOver(highScorerData["player"]);
            isShowGameOverRevealed = true;
        }
    }


    /**
     * When game is over, instantiate a GameOverView object, pass it the winner, and position it. 
     * Let the GameOverView object handle the rest.
     */ 
    private void ShowGameOver(int winnerNum = 1) 
    {
        GameObject[] playerViewObjects = GameObject.FindGameObjectsWithTag("Left Camera");

        foreach (GameObject playerView in playerViewObjects) {

            GameObject gameOver = Instantiate(
                gameOverView, 
                new Vector3(0f, 0f, 0f),
                Quaternion.identity
            ) as GameObject;

            GameOver gameOverScript = gameOver.GetComponent<GameOver>();

            gameOverScript.SetWinner(winnerNum);
            gameOver.transform.parent = playerView.transform;
            gameOver.transform.localPosition = new Vector3(-4f, 2f, 8f);
            gameOver.transform.rotation = playerView.transform.rotation;
//            gameOverScript.setPlayerViewObject(playerView);
//            gameOverScript.setTransform();

//            StartCoroutine(restartAfterTime(GAME_CONFIG.restartDelay));
        }
    }
}
