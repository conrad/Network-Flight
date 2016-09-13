using UnityEngine;

public class ObjectPlacer
{
    public float pickUpPositionY = 50.0f;
    public Vector3 frontTopLeftCorner;
    public Vector3 backLowerRighCorner;



//    public ObjectPlacer(
//        Vector3 corner1,
//        Vector3 corner2
//    ) {
//        frontTopLeftCorner = corner1;
//        backLowerRighCorner = corner2;
//    }



    // Find a suitable position for an object above the terrain and within the environment.
    public Vector3 GenerateGameObjectPosition(int attemptLimit = 50) 
    {
        bool placed = false;
        int attempts = 0;
//        int pickUpPositionYInt = (int) pickUpPositionY;
        float relPickUpHeight = Random.Range(2.0f, 50.0f);

        float pickUpPositionX = Random.Range(-450.0f, 450.0f);
        float pickUpPositionZ = Random.Range(-450.0f, 450.0f);
        Vector3 pickUpPosition = new Vector3(pickUpPositionX, pickUpPositionY, pickUpPositionZ);;

        // Loop lowering items each time until it is the proper distance from a collider below.
        while (!placed && attempts < attemptLimit) {
            pickUpPosition = new Vector3(pickUpPositionX, pickUpPositionY, pickUpPositionZ);
            RaycastHit hit;
            Vector3 direction = Vector3.down * relPickUpHeight; // new Vector3(0f, -relPickUpHeight, 0f);
//            Ray placementRay = new Ray(pickUpPosition, direction);

//            if (Physics.Raycast(pickUpPosition, direction, relPickUpHeight + 2f)) {
//            if (Physics.Raycast(placementRay, out hit, relPickUpHeight + 2f)) {
            if (Physics.Raycast(pickUpPosition, direction, out hit)) {
//                if (hit.collider.tag == "Ground") {
                    placed = true;
                    Debug.Log("SHAZAM! Ray hit & placed at " + pickUpPositionY);
//                }
            }

            if (pickUpPositionY < relPickUpHeight + 1f) {
                Debug.Log("Oh no! Somehow Pick Up fell too far to height: " + pickUpPositionY);
                attempts += 200;
            }

            pickUpPositionY -= 1f;
            attempts++;
        }

        if (!placed) {
            pickUpPosition = new Vector3(pickUpPositionX, Random.Range(10.0f, 200.0f), pickUpPositionZ);;
        }

        return pickUpPosition;
    }

}

