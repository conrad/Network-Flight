using UnityEngine;

/**
 * Values to be shared across the game.
 */ 
public class GameConfig
{
    public static GameConfig _instance = null;
    public string defaultRoomName = "BuddyRace";
    public int numPlayersForGame = 1;
    public float playerSpeed = 30f;
    public int totalPickUps = 11;
    public float restartDelay = 10.0f;
    public string VERSION = "v0.0.1";
    public int playerNumber;
    public bool isSoloGame = true;      // TODO: Change this value to false for multiplayer.
    public Vector3 farTopRightCorner = new Vector3(400f, 700f, 400f);
    public Vector3 nearBottomLeftCorner = new Vector3(-400f, 0f, -400f);

    private bool isInstantiated = false;


    // TODO: Create public getters for all of these variables and make the variables private.



    public GameConfig() 
    {
        if (isInstantiated) 
        {
            Debug.Log("Trying to call GameConfig constructor a second time. This should be called only once.");
        }

        isInstantiated = true;
    }



    /* singleton */
    public static GameConfig Instance() { 

        if (_instance == null)
        {
            _instance = new GameConfig() ;
        }
        return _instance;
    }



    public void setPlayerNumber(int playerNum)
    {
        playerNumber = playerNum;
    }
}
