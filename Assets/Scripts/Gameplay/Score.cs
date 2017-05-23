using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * A class to manage the view of the score view for each player.
 */
public class Score : Photon.MonoBehaviour, IScore
{
    public int playerNumber;
    public TextMesh scoreTextMesh;
    private string text;



    public void UpdateScoreView(int scorePlayerNumber, int newScore)   // int increment = 1
    {
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
    