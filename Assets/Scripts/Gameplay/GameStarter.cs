using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameStarter : MonoBehaviour 
{
    public GameObject photonNetworkManager;
    public GameObject spawnPoint;
    public GameObject initialCamera;
    public GameObject playerPrefab; 
    public GameObject pickUpPrefab;
    public GameObject scorePrefab;
    public float pickUpPositionY = 50.0f;
    public float loadDelay = 10.0f;
    public string roomName = "";
    //Toolkit instance.
    [SerializeField]TerrainToolkit kit;
    //Array of textures.
    [SerializeField]Texture2D sandTexture;
    [SerializeField]Texture2D grassTexture;
    [SerializeField]Texture2D rockTexture;
    [SerializeField]Texture2D cliffTexture;

    GameConfig GameConfig;

//    IEnumerator Start()
//    {
//        yield return StartCoroutine(MyDelayMethod(3.1415f));
//        //3.1415 seconds later
//    }
//
//    IEnumerator MyDelayMethod(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//    }


    void Start()
    {
        GameConfig = GameConfig.Instance();  // Use this method make sure you use singleton.

        if (!GameConfig.isSoloGame) {
            if (GameObject.Find("PhotonNetworkManager") == null) {
                Instantiate(photonNetworkManager, Vector3.zero, Quaternion.identity);
            }

//            StartCoroutine(DelayForLoading());
//            GenerateTerrain();

            if (PhotonNetwork.inRoom) {
                AddPlayer();    
            }

            // TODO: FIND OUT - Is this happening before the TERRAIN EXISTS???
            if (PhotonNetwork.isMasterClient) {
                AddPickUps();
            }
        } else {
//            GenerateTerrain();
            AddPlayer(GameConfig.isSoloGame);    
            AddPickUps(GameConfig.isSoloGame);
        }
    }



//    void GenerateTerrain()
//    {
//        GameObject terrain = GameObject.Find("Environment/Terrain");
//
//        if (!terrain) 
//        {
//            Debug.Log("Error: Could not locate Terrain GameObject.");
//            return;
//        }
//
//        PerlinTerrain terrainScript = terrain.GetComponent<PerlinTerrain>();
//
//        if (!terrainScript) 
//        {
//            Debug.Log("Error: Could not locate PerlinTerrain component");
//            return;
//        }
//
//        terrainScript.PrepareAndGenerateTerrain();
//    }



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



//    IEnumerator DelayForLoading()
//    {
//        Debug.Log("WHY AREN'T YOU BEING CALLED?");
//        print(Time.time);
//        Debug.Log("KJDHSFHJKSDFHJKSDFKHJSDFHJK");
//        yield return new WaitForSeconds(loadDelay);
//        print(Time.time);
//    }
}
