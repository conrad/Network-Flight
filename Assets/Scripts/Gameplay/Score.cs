using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Score : Photon.MonoBehaviour 
{
    public int playerNumber;
    public TextMesh scoreTextMesh;
    private string text;



    public void UpdateScoreView(int scorePlayerNumber, int newScore)   // int increment = 1
    {
        Debug.Log("updateScoreView called in Score");
        if (scorePlayerNumber == playerNumber) {
            text = "Player " + playerNumber + ": " + newScore;
            scoreTextMesh.text = text;
        }
    }



    public int GetPlayerNumber() {
        return playerNumber;
    }



    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) {
            stream.SendNext(text);
        } else {
            scoreTextMesh.text = (string)stream.ReceiveNext();
        }
    }
}
    