using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * A class to manage the Heads-Up Display version of the score view for each player.
 */
public class HUDScore : Photon.MonoBehaviour, IScore 
{
	public int playerNumber;

	private TextMesh scoreTextMesh;
	private string text;


	void Start()
	{
		scoreTextMesh = gameObject.GetComponent<TextMesh>();
	}


	public void UpdateScoreView(int scorePlayerNumber, int newScore)   // int increment = 1
	{
		if (scorePlayerNumber == playerNumber) {
			scoreTextMesh.text = newScore.ToString();
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
