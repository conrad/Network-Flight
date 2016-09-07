using UnityEngine;
using System.Collections;

public class FlatRotator : MonoBehaviour {

    public float speed = 100;
    public string axis = "y";
    public float delay = 0.0f;



    void Update () 
    {
        RotateObject();
    }



    void RotateObject()
    {
        if (Time.fixedTime > delay) {
            if (axis.ToLower() == "y") {
                transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
            } else if (axis.ToLower() == "x") {
                transform.Rotate(new Vector3(speed, 0, 0) * Time.deltaTime);
            } else if (axis.ToLower() == "z") {
                transform.Rotate(new Vector3(0, 0, speed) * Time.deltaTime);
            }
        }
    }
}