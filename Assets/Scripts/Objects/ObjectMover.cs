using UnityEngine;
using System.Collections;

/**
 * This class moves objects after a given target, unlike Mover, which just moves objects forward.
 */ 
public class ObjectMover : MonoBehaviour 
{
    public Transform target;
    public float speed = 1f;



    void Update() 
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
