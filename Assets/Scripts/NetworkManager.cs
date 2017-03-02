using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;	


public class NetworkManager : Photon.MonoBehaviour 
{
    public GameObject ObjectHandler;
    public InputField roomInput;
    public string roomName = "";

	GameConfig gameConfig;
    MenuObjectHandler menuObjectHandler;
    int numberOfPlayers = 0;


	void Awake()
	{
		menuObjectHandler = ObjectHandler.GetComponent<MenuObjectHandler>();
	}



    void OnDestroy()
    {
		if (SceneManager.GetActiveScene().name == "Desert") {      //	|| SceneManager.GetActiveScene().name == "TransitionToMenu") 
            SceneManager.LoadScene("Menu");
        }
    }



    void OnJoinedLobby() {
        Debug.Log("Joined Lobby");
		menuObjectHandler.MakeTransitionOnJoinedLobby();
    }



	public void ChooseSinglePlayer()
	{
		gameConfig = GameConfig.Instance(); 	  // Use the instance method to ensure use of GameConfig singleton.
		gameConfig.isSoloGame = true;

		menuObjectHandler.PrepareForSoloGame();
	}



	public void ChooseMultiplayer() 
	{
		DontDestroyOnLoad(transform.gameObject);
		gameConfig = GameConfig.Instance(); 	  // Use the instance method to ensure use of GameConfig singleton.
		gameConfig.isSoloGame = false;
		roomName = gameConfig.defaultRoomName;

		menuObjectHandler.HideModeButtons();
	
		try {
			PhotonNetwork.ConnectUsingSettings(gameConfig.VERSION);
		} 
		catch (Exception e) {
			// Create message for failure here: "Error: Unable to connect to internet."
			Debug.Log("Caught the exception here: " + e.Message);
		}
	}



    public void JoinRoom() 
    {
        if (SceneManager.GetActiveScene().name == "Menu") {
            if (roomInput.text.Length > 0) {
				roomName = roomInput.text;
            }
            if (roomName.Length > 0) {          // Do this check separately in case default room name is being used.
                Debug.Log("Joining room...");
                menuObjectHandler.MakeTransitionPreJoinRoom();

                RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 2 };
                bool hasJoined = PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);

                if (!hasJoined) {
                    menuObjectHandler.MakeTransitionBackToLobby();
                }
            }
        }
    }



    void OnJoinedRoom()
    {
        int playerNumber = GetNumberOfPlayers();

        if (SceneManager.GetActiveScene().name == "Menu") {
			gameConfig.setPlayerNumber(playerNumber);
            menuObjectHandler.MakeTransitionOnJoinedRoom(playerNumber);
        }
    }



    void OnPhotonPlayerDisconnected()
    {
        if (SceneManager.GetActiveScene().name == "Menu") {
            menuObjectHandler.MakeTransionOnLeaveRoom();
            PhotonNetwork.LeaveRoom();
        }
    }



    void OnPhotonPlayerConnected()
    {
        if (SceneManager.GetActiveScene().name == "Menu") {
            int tempPlayerNum = PhotonNetwork.playerList.Length;
            if (tempPlayerNum != numberOfPlayers) {
                Debug.Log("OnPhotonPlayerConnected called");
                numberOfPlayers = tempPlayerNum;
                menuObjectHandler.UpdatePlayerNumber(numberOfPlayers);
            }
        }

    }


    
    public void StartGame() 
    {
		if (PhotonNetwork.connected) {
			if (PhotonNetwork.playerList.Length >= gameConfig.numPlayersForGame) {
				EnterMultiplayerGame();
			}
		} else {
			EnterSoloGame();
        }
    }



	void EnterMultiplayerGame()
	{
		PhotonNetwork.room.open = false;
		PhotonNetwork.LoadLevel("Desert");
	}



	void EnterSoloGame()
	{
		SceneManager.LoadScene ("Desert");
	}



	public void BackToModeChoice()
	{
		menuObjectHandler.TransitionToStart();

		if (PhotonNetwork.connected) {
			PhotonNetwork.Disconnect();
		}
	}


    public void BackToJoin()
    {
        menuObjectHandler.MakeTransionOnLeaveRoom();
        PhotonNetwork.LeaveRoom();
    }



    public int GetNumberOfPlayers() 
    {
        return PhotonNetwork.playerList.Length;
    }



    public void GoToInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }
}
