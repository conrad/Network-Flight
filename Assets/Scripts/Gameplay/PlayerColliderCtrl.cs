using UnityEngine;
using System.Collections;

public class PlayerColliderCtrl : MonoBehaviour {

    public int totalPickUps = 11;
    private int count = 0;

    void OnCollisionEnter(Collision collision) {

        Debug.Log("collision with" + collision.gameObject.name);

        if (collision.gameObject.name == "Pick Up") 
//        if (collision.gameObject.CompareTag("Pick Up")) 
        {
            collision.gameObject.SetActive (false);
            Debug.Log("Got one!");
            //
            count++;

            if (count >= totalPickUps) {
                Debug.Log(count);
                Debug.Log("YOU WIN!");
            }

            //            SetCountText ();
            //            if (count >= 9) 
            //            {
            //                winText.text = "You Win!";
            //            }
        }
    }
}
