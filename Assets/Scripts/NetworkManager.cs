using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class NetworkManager : MonoBehaviour 
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

        GameConfig = GameConfig.Instance();     // Use the instance method to ensure singleton.
        roomName = GameConfig.defaultRoomName;

        PhotonNetwork.automaticallySyncScene = true; 
    }



    void Start() 
    {
        if (SceneManager.GetActiveScene().name == "Menu") {
            menuObjectHandler = ObjectHandler.GetComponent<MenuObjectHandler>();
            PhotonNetwork.ConnectUsingSettings(GameConfig.VERSION);
        }
    }



    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu") {
            if (PhotonNetwork.playerList.Length > numberOfPlayers) {
                Debug.Log("players on network: " + PhotonNetwork.playerList.Length);
                numberOfPlayers = PhotonNetwork.playerList.Length;
                menuObjectHandler.UpdatePlayerNumber(numberOfPlayers);
            }
        }
    }



    void OnJoinedLobby() {
        Debug.Log("Joined Lobby");
        if (SceneManager.GetActiveScene().name == "Menu") {
            menuObjectHandler.TransitionOnJoinedLobby();
        }
    }



    public void JoinRoom() 
    {
        Debug.Log("Joining room...");
        if (SceneManager.GetActiveScene().name == "Menu") {
            if (roomInput.text.Length > 0) {
                roomName = roomInput.text;
            }
        }

        menuObjectHandler.TransitionPreJoinRoom();

        RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }



    void OnJoinedRoom()
    {
        int playerNumber = GetNumberOfPlayers();

        if (SceneManager.GetActiveScene().name == "Menu") {
            GameConfig.setPlayerNumber(playerNumber);
            menuObjectHandler.TransitionOnJoinedRoom(playerNumber);
        }

        if (SceneManager.GetActiveScene().name == "Desert") {
            // TODO: Prepare the scene if join when already in the desert...
            Debug.Log("Joined Room while in Desert scene");

            // call methods to addPlayer & addPickups...
        }
    }



    public void StartGame() 
    {
        if (PhotonNetwork.playerList.Length >= GameConfig.numPlayersForGame) {
//            SceneManager.LoadScene("Desert");
            PhotonNetwork.LoadLevel("Desert");
//            PhotonView.RPC("StartNewLevel", PhotonTargets.All,"blahblahblahPlaygroundScene");

        } else {
            Debug.Log("You need more players to play...");
        }
    }



    public void Back()
    {
        menuObjectHandler.TransionOnLeaveRoom();
        PhotonNetwork.LeaveRoom();
    }



    public int GetNumberOfPlayers() 
    {
        return PhotonNetwork.playerList.Length;
    }
}
