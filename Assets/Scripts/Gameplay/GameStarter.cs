﻿using UnityEngine;
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
    public GameObject spawnPoint;
    public Transform[] spawnPoints;
    public float pickUpPositionY = 50.0f;
    public float loadDelay = 10.0f;
    public string roomName = "";
    //Array of textures.
    [SerializeField]Texture2D sandTexture;
    [SerializeField]Texture2D grassTexture;
    [SerializeField]Texture2D rockTexture;
    [SerializeField]Texture2D cliffTexture;

    GameConfig GameConfig;
    ObjectPlacer objectPlacer;

    void Start()
    {
        GameConfig = GameConfig.Instance();  // Use this method make sure you use singleton.
        objectPlacer = new ObjectPlacer();

        if (!GameConfig.isSoloGame) {
            if (PhotonNetwork.inRoom) {
                AddPlayer(GameConfig.isSoloGame);    
            }

            if (PhotonNetwork.isMasterClient) {
                AddPickUps(GameConfig.isSoloGame);
            }
        } else {
            AddPlayer(GameConfig.isSoloGame);    
            AddPickUps(GameConfig.isSoloGame);
        }
    }
        


    void AddPlayer(bool isSoloGame = false) {
        initialCamera.SetActive(false);

//        Vector3 playerPosition = objectPlacer.GenerateRandomObjectPosition(
//            new Vector3(400f, 700f, 400f),
//            new Vector3(-400f, 0f, -400f),
//            5f
//        );

        Vector3 playerPosition = FindPlayerPosition();

        if (!isSoloGame) {
            GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, playerPosition, Quaternion.identity, 0);
            newPlayer.GetComponent<PlayerController>().playerNumber = GameConfig.playerNumber;
        } else {
//            GameObject newPlayer = Instantiate(playerPrefab);
            GameObject newPlayer = Instantiate(playerPrefab, playerPosition, Quaternion.identity) as GameObject;
            newPlayer.transform.FindChild("Suited Man").position = newPlayer.transform.position;
            newPlayer.transform.FindChild("GvrMain").position = newPlayer.transform.position;
            newPlayer.transform.FindChild("PlayerCollider").position = newPlayer.transform.position;


            newPlayer.GetComponent<PlayerController>().playerNumber = 1;
        }
      
        AddPlayerScore(GameConfig.playerNumber, GameConfig.isSoloGame);
    }



    // Add Player's Score to the Scene.
    void AddPlayerScore(int playerNumber, bool isSoloGame = false) 
    {
        if (!isSoloGame) {
            GameObject playerScore = PhotonNetwork.Instantiate(
                                         scorePrefab.name, 
                                         new Vector3(0f, 700f + (playerNumber * 40), 0f),
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
        Debug.Log("Adding " + GameConfig.totalPickUps + " pick ups to scene");
        for (var i = 0; i < GameConfig.totalPickUps; i++) {
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
        int playerNum = GameConfig.playerNumber;

        Vector3 topPlayerPos = new Vector3(
            spawnPoint.transform.position.x,        //  spawnPoints[playerNum-1].position.x,
            GameConfig.farTopRightCorner.y,
            spawnPoint.transform.position.z         //  spawnPoints[playerNum-1].position.z
        );

        float playerHeight = objectPlacer.GenerateObjectHeight(
            topPlayerPos,   //    Vector3 attemptPosition
            50f,            //    float targetHeightFromGround,
            GameConfig.farTopRightCorner.y / 2,     //    float defaultHeight,
            5f,     //    float step, 
            50      //    float attemptsLeft
        );
            
        return new Vector3(
            spawnPoint.transform.position.x,    //  spawnPoints[0].position.x, 
            playerHeight,
            spawnPoint.transform.position.z     //  spawnPoints[0].position.z
        );
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
