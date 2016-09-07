using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;



public class GameStarter : MonoBehaviour 
{
    public GameObject photonNetworkManager;
    public GameObject spawnPoint;
    public GameObject initialCamera;
    public GameObject playerPrefab; 
    public GameObject pickUpPrefab;
    public GameObject scorePrefab;
    public float pickUpPositionY = 50.0f;
    public string roomName = "";

    GameConfig GameConfig;



    void Start()
    {
        GameConfig = GameConfig.Instance();  // Use this method make sure you use singleton.

        if (!GameConfig.isSoloGame) {
            if (GameObject.Find("PhotonNetworkManager") == null) {
                Instantiate(photonNetworkManager, Vector3.zero, Quaternion.identity);
            }


            if (PhotonNetwork.inRoom) {
                AddPlayer();    
            }

            if (PhotonNetwork.isMasterClient) {
                AddPickUps();
            }
        } else {
            AddPlayer(GameConfig.isSoloGame);    
            AddPickUps(GameConfig.isSoloGame);
        }
    }



    void AddPlayer(bool isSoloGame = false) {
        initialCamera.SetActive(false);

        Debug.Log("Spawn Point: " + spawnPoint);
        Debug.Log("Spawn Point position: " + spawnPoint.transform.position);
        if (!isSoloGame) {
            GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
            newPlayer.GetComponent<PlayerController>().playerNumber = GameConfig.playerNumber;
        } else {
            GameObject newPlayer = GameObject.Instantiate(playerPrefab);
            newPlayer.transform.position = spawnPoint.transform.position;
            newPlayer.GetComponent<PlayerController>().playerNumber = 1;
        }

//        Debug.Log("actual initial position of newPlayer: " + newPlayer.transform.position);
//        Debug.Log("newPlayer: " + newPlayer);
//        Debug.Log("actual playerNumber " + newPlayer.GetComponent<PlayerController>().playerNumber);
        Debug.Log("config playerNumber " + GameConfig.playerNumber);
        AddPlayerScore(GameConfig.playerNumber, GameConfig.isSoloGame);
    }



    void AddPlayerScore(int playerNumber, bool isSoloGame = false) {
        // Add Player's Score to Scene.
        if (!isSoloGame) {
            GameObject playerScore = PhotonNetwork.Instantiate(
                                         scorePrefab.name, 
                                         new Vector3(0f, 700f + (playerNumber * 40), 0f),
                                         spawnPoint.transform.rotation,
                                         0
                                     );

            playerScore.GetComponent<Score>().playerNumber = playerNumber;
        } else {
            GameObject playerScore = Instantiate(scorePrefab);

            playerScore.GetComponent<Score>().playerNumber = 1;
        }
    }



    void AddPickUps(bool isSoloGame = false) {
        Debug.Log("Adding " + GameConfig.totalPickUps + " pick ups to scene");
        for (var i = 0; i < GameConfig.totalPickUps; i++) {

            ObjectPlacer objectPlacer = new ObjectPlacer();
            Vector3 pickUpPosition = objectPlacer.GenerateGameObjectPosition();
            if (!isSoloGame) {
                PhotonNetwork.Instantiate(
                    pickUpPrefab.name, 
                    pickUpPosition,
                    Quaternion.identity,
                    0
                );
            } else {
                GameObject pickUp = Instantiate(pickUpPrefab);
                pickUp.transform.position = pickUpPosition;
            }
        }
    }
}







//
//Here's code for both catching the submission (this happens when you press a button that is set as Submit in the input system - Enter by default) and using a button to submit:
//
//(include UnityEngine.UI for access to UI components)
//
//[SerializeField]
//private InputField nameInputField = null;
//
//[SerializeField]
//private Button submitButton = null;
//
//private void Start()
//{
//    // Add listener to catch the submit (when set Submit button is pressed)
//    nameInputField.onSubmit.AddListener((value) => SubmitName(value));
//    // Add validation
//    nameInputField.validation = InputField.Validation.Alphanumeric;
//
//    // This is a setup for a button that grabs the field value when pressed
//    submitButton.onClick.AddListener(() => SubmitName(nameInputField.value));
//}
//
//private void SubmitName(string name)
//{
//    //What to do with the value from input field
//}
