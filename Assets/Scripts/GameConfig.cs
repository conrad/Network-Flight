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


    // TODO: Create public getters for all of these variables and make the variables private.



    public GameConfig() 
    {
        Debug.Log("GameConfig constructor. This should be called only once.");
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
