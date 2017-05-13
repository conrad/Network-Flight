using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NetworkManagerRemover : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        PhotonNetwork.Disconnect ();
        GameObject networkManager = GameObject.Find("PhotonNetworkManager");
        Destroy(networkManager);
	}
}
