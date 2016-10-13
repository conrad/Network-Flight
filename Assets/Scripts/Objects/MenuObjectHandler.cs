using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuObjectHandler : MonoBehaviour 
{
    public GameObject pickUp;
    public GameObject roomInput;
    public GameObject joinButton;
    public GameObject avatar;
    public GameObject startButton;
    public GameObject backButton;
    public Text playerNumberView;
    public Text totalPlayersView;
    public Text waitingMessageView;
    public Text instructions;

    int playerNumber;
    int totalPlayers;
    GameObject avatarObject;

    public void TransitionOnJoinedLobby()
    {
        roomInput.SetActive(true);
        joinButton.SetActive(true);
    }



    public void TransitionPreJoinRoom()
    {
        roomInput.SetActive(false);
        joinButton.SetActive(false);
        instructions.GetComponent<Text>().enabled = false;
    }



    public void TransitionOnJoinedRoom(int playerNum)
    {
        // Remove room inputField, pickUp & button
        pickUp.SetActive(false);

        // Add prefab of avatar and activate Start button and ready text.
        avatarObject = Instantiate(avatar, new Vector3(-21.5f, 50f, 10.1f), Quaternion.identity) as GameObject;

//        if (PhotonNetwork.isMasterClient) {
            startButton.SetActive(true);        // had difficulty instantiating UI object
            backButton.SetActive(true); 
//        } else {
//            waitingMessageView.GetComponent<Text>().enabled = true;
//        }

        playerNumber = playerNum;
        totalPlayers = playerNum;
        playerNumberView.text = "READY PLAYER " + playerNumber;
        totalPlayersView.text = totalPlayers + " PLAYERS TOTAL";
        playerNumberView.GetComponent<Text>().enabled = true;
        totalPlayersView.GetComponent<Text>().enabled = true;
    }



    public void UpdatePlayerNumber(int numberOfPlayers)
    {
        totalPlayers = numberOfPlayers;
        totalPlayersView.text = totalPlayers + " players total";
    }



    public void TransionOnLeaveRoom()
    {
        // Remove Joined Objects
        startButton.SetActive(false); 
        backButton.SetActive(false); 
        avatarObject.SetActive(false);
        playerNumberView.GetComponent<Text>().enabled = false;
        totalPlayersView.GetComponent<Text>().enabled = false;

        // Replace Initial Objects
        roomInput.SetActive(true);
        joinButton.SetActive(true);
        instructions.GetComponent<Text>().enabled = true;
        pickUp.SetActive(true);
    }
}
    