using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class NetworkManager : Photon.MonoBehaviour 
{
    public GameObject ObjectHandler;
    public InputField roomInput;
    public string roomName = "";

    GameConfig GameConfig;
    MenuObjectHandler menuObjectHandler;
    int numberOfPlayers = 0;



    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        GameConfig = GameConfig.Instance();     // Use the instance method to ensure GameConfig singleton.
//        roomName = GameConfig.defaultRoomName;
    }



    void Start() 
    {
        if (SceneManager.GetActiveScene().name == "Menu") {
            menuObjectHandler = ObjectHandler.GetComponent<MenuObjectHandler>();
            PhotonNetwork.ConnectUsingSettings(GameConfig.VERSION);
        }
    }



    void OnDestroy()
    {
        SceneManager.LoadScene("Menu");
    }



    void OnJoinedLobby() {
        Debug.Log("Joined Lobby");
        if (SceneManager.GetActiveScene().name == "Menu") {
            menuObjectHandler.MakeTransitionOnJoinedLobby();
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
            GameConfig.setPlayerNumber(playerNumber);
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



    void EnterDesert()
    {
        PhotonNetwork.room.open = false;
        PhotonNetwork.LoadLevel("Desert");
    }



    public void StartGame() 
    {
        if (PhotonNetwork.playerList.Length >= GameConfig.numPlayersForGame) {
            EnterDesert();
        }
    }



    public void Back()
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
