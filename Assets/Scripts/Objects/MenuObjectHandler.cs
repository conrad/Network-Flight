﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuObjectHandler : MonoBehaviour 
{
    public GameObject pickUp;
    public GameObject roomInput;
    public GameObject joinButton;
    public GameObject instructionsButton;
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
        instructionsButton.SetActive(true);
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
        avatarObject.transform.localScale = new Vector3(3f, 3f, 3f);

        startButton.SetActive(true); 
        backButton.SetActive(true); 

        playerNumber = playerNum;
        totalPlayers = playerNum;
        playerNumberView.text = "READY PLAYER " + playerNumber;
        SetTotalPlayersView(totalPlayers);
        playerNumberView.GetComponent<Text>().enabled = true;
        totalPlayersView.GetComponent<Text>().enabled = true;
    }



    private void SetTotalPlayersView(int totalNumberOfPlayers)
    {
        if (totalPlayers > 1) {
            totalPlayersView.text = totalPlayers + " PLAYERS TOTAL";
        } else {
            totalPlayersView.text = "1 PLAYER PRESENT";
        }
    }



    public void UpdatePlayerNumber(int numberOfPlayers)
    {
        totalPlayers = numberOfPlayers;
        SetTotalPlayersView(totalPlayers);
    }



    public void TransionOnLeaveRoom()
    {
        // Remove OnJoinedRoom Objects
        startButton.SetActive(false); 
        backButton.SetActive(false); 
        Destroy(avatarObject);
        playerNumberView.GetComponent<Text>().enabled = false;
        totalPlayersView.GetComponent<Text>().enabled = false;

        // Replace Initial Objects
        roomInput.SetActive(true);
        joinButton.SetActive(true);
        instructions.GetComponent<Text>().enabled = true;
        pickUp.SetActive(true);
    }
}
    