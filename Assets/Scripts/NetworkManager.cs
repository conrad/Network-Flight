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

        GameConfig = GameConfig.Instance();     // Use the instance method to ensure singleton.
        roomName = GameConfig.defaultRoomName;
    }



    void Start() 
    {
        if (SceneManager.GetActiveScene().name == "Menu") {
            menuObjectHandler = ObjectHandler.GetComponent<MenuObjectHandler>();
            PhotonNetwork.ConnectUsingSettings(GameConfig.VERSION);
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
    }



    void OnPhotonPlayerDisconnected()
    {
        if (SceneManager.GetActiveScene().name == "Menu") {
            menuObjectHandler.TransionOnLeaveRoom();
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



    [PunRPC]
    void EnterDesert()
    {
        PhotonNetwork.LoadLevel("Desert");
    }



    public void StartGame() 
    {
        if (PhotonNetwork.playerList.Length >= GameConfig.numPlayersForGame) {
            EnterDesert();
//            this.gameObject.AddComponent<PhotonView>();
//            PhotonView photonView = PhotonView.Get(this);
//            photonView.RPC("EnterDesert", PhotonTargets.All);
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



    public void GoToInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }
}
