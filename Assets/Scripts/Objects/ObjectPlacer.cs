using UnityEngine;



public class ObjectPlacer
{
    // Find a random position within a box environment above a randomly generated terrain.
    public Vector3 GenerateRandomObjectPosition(
        Vector3 farTopRightCorner, 
        Vector3 nearBottomLeftCorner, 
        float objectRadius
    ) {
        float positionX = Random.Range(farTopRightCorner.x - objectRadius, nearBottomLeftCorner.x + objectRadius);
        float positionZ = Random.Range(farTopRightCorner.z - objectRadius, nearBottomLeftCorner.z + objectRadius);
        float targetHeightFromGround = Random.Range(
            farTopRightCorner.y / 4, 
            nearBottomLeftCorner.y + objectRadius
        );

        float positionY = GenerateObjectHeight(
            new Vector3(positionX, farTopRightCorner.y - objectRadius, positionZ),
            targetHeightFromGround,
            farTopRightCorner.y / 2.5f,
            objectRadius, 
            farTopRightCorner.y / objectRadius
        );

        return new Vector3(positionX, positionY, positionZ);
    }



    // Find an appropriate height above a dynamic terrain.
    public float GenerateObjectHeight(
        Vector3 attemptPosition, 
        float targetHeightFromGround,
        float defaultHeight,
        float step, 
        float attemptsLeft
    ) {
        RaycastHit hit;
        Vector3 direction = Vector3.down;           // new Vector3(0f, -targetHeightFromGround, 0f);       // Vector3.down * targetHeightFromGround;          // 

        if (Physics.Raycast(attemptPosition, direction, out hit, targetHeightFromGround)) 
        {
            return attemptPosition.y;
        }

        if (attemptPosition.y <= 0f || attemptsLeft <= 0f)
        {
            Debug.Log("Couldn't find proper position. Placing object at default height.");
            return defaultHeight;
        }

        return GenerateObjectHeight(
            new Vector3(attemptPosition.x, attemptPosition.y - step, attemptPosition.z),
            targetHeightFromGround,
            defaultHeight,
            step, 
            attemptsLeft - 1
        ); 
    }
}
