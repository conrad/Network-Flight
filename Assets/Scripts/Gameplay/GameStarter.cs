using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameStarter : MonoBehaviour 
{
    public GameObject photonNetworkManager;
    public GameObject initialCamera;
    public GameObject playerPrefab; 
    public GameObject pickUpPrefab;
    public GameObject scorePrefab;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public float pickUpPositionY = 50.0f;
    public float loadDelay = 10.0f;
    public string roomName = "";

    //Array of textures.
    [SerializeField]Texture2D sandTexture;
    [SerializeField]Texture2D grassTexture;
    [SerializeField]Texture2D rockTexture;
    [SerializeField]Texture2D cliffTexture;

    GameConfig gameConfig;
    ObjectPlacer objectPlacer;
    string avatarName = "Sports car 1";



    void Start()
    {
        gameConfig = GameConfig.Instance();  // Use this method make sure you use singleton.
        objectPlacer = new ObjectPlacer();

		Debug.Log ("isSoloGame: " + gameConfig.isSoloGame);

        if (!gameConfig.isSoloGame) {
            if (PhotonNetwork.inRoom) {
                AddPlayer(gameConfig.isSoloGame);    
            }

            if (PhotonNetwork.isMasterClient) {
                AddPickUps(gameConfig.isSoloGame);
            }
        } else {
            AddPlayer(gameConfig.isSoloGame);    
            AddPickUps(gameConfig.isSoloGame);
        }
    }
        


    void AddPlayer(bool isSoloGame = false) {
        initialCamera.SetActive(false);

        Vector3 playerPosition = FindPlayerPosition();

        if (!isSoloGame) {
            GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, playerPosition, Quaternion.identity, 0);
            newPlayer.GetComponent<PlayerController>().playerNumber = gameConfig.playerNumber;

        } else {
            GameObject newPlayer = Instantiate(playerPrefab, playerPosition, Quaternion.identity) as GameObject;
            newPlayer.GetComponent<PlayerController>().playerNumber = 1;
        }
      
        AddPlayerScore(gameConfig.playerNumber, gameConfig.isSoloGame);
    }



    void SetRendererColor(Renderer rend, int variation)
    {
        switch (variation) 
        {
            case 1:
                rend.material.SetColor("_Color", Color.blue);
                break;
            case 2: 
                rend.material.SetColor("_Color", Color.red);
                break;
            default:
                rend.material.SetColor("_Color", Color.blue);
                break;
        }
    }



    // Add Player's Score to the Scene.
    void AddPlayerScore(int playerNumber, bool isSoloGame = false) 
    {
        if (!isSoloGame) {
            GameObject playerScore = PhotonNetwork.Instantiate(
                                         scorePrefab.name, 
                                         new Vector3(0f, 580f + (playerNumber * 120), 0f),
                                         Quaternion.identity,
                                         0
                                     );

            playerScore.GetComponent<Score>().playerNumber = playerNumber;
        
        } else {
            GameObject playerScore = Instantiate(scorePrefab);

            playerScore.GetComponent<Score>().playerNumber = 1;
        }
    }



    // Add the items to pick up to the Scene.
    void AddPickUps(bool isSoloGame = false) 
    {
        Debug.Log("Adding " + gameConfig.totalPickUps + " pick ups to scene");
        for (var i = 0; i < gameConfig.totalPickUps; i++) {
            Vector3 pickUpPosition = objectPlacer.GenerateRandomObjectPosition(
                new Vector3(400f, 700f, 400f),
                new Vector3(-400f, 0f, -400f),
                5f
            );

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



    private Vector3 FindPlayerPosition()
    {
        GameObject spawn = GetSpawnPoint();

        Vector3 topPlayerPos = new Vector3(
            spawn.transform.position.x, 
            gameConfig.farTopRightCorner.y - 10f,
            spawn.transform.position.z 
        );

        float playerHeight = objectPlacer.GenerateObjectHeight(
            topPlayerPos,   //    Vector3 attemptPosition
            50f,            //    float targetHeightFromGround,
            gameConfig.farTopRightCorner.y / 2,     //    float defaultHeight,
            5f,     //    float step, 
            50      //    float attemptsLeft
        );
            
        return new Vector3(
            spawn.transform.position.x, 
            playerHeight,
            spawn.transform.position.z 
        );
    }



    private GameObject GetSpawnPoint()
    {
        if (gameConfig.playerNumber >= 2) {
            return spawnPoint2;
        }

        return spawnPoint1;
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
//        TerrainToolkit kit = terrain.GetComponent<TerrainToolkit>();
//
//        if (!kit) 
//        {
//            Debug.Log("Error: Could not locate PerlinTerrain component");
//            return;
//        }
//
//        //Generate the perlin terrain.
//        kit.PerlinGenerator((int)Random.Range(3,6),Random.Range(0.4f,0.9f),Random.Range(2,6), 1f);
//        //Gives it a less smooth feel.
//        kit.PerlinGenerator(4,4,4, 0.1f);
//        //Creates arrays for stops.
//        float[] slopeStops = new float[2];
//        float[] heightStops = new float[4];
//        Texture2D[] terrainTextures = new Texture2D[4];
//        //Assigns values to the arrays.
//        slopeStops[0] = 30f; slopeStops[1] = 70f;
//        heightStops[0] = Random.Range(0.05f, 0.18f);
//        heightStops[1] = Random.Range(0.19f, 0.49f);
//        heightStops[2] = Random.Range(0.5f, 0.69f);
//        heightStops[3] = Random.Range(0.7f, 0.89f);
//        terrainTextures[0] = cliffTexture;
//        terrainTextures[1] = sandTexture;
//        terrainTextures[2] = grassTexture;
//        terrainTextures[3] = rockTexture;
//        //Paints the textures.
//        kit.TextureTerrain(slopeStops, heightStops, terrainTextures);
//        Debug.Log("DONE GENERATING TERRAIN");
//    }
