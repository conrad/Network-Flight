using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class ModalController : MonoBehaviour 
{
    public GameObject modal;
    public GameObject quitButton;
    public GameObject resumeButton;

     

    void Update()
    {
        if (modal.activeSelf) {
            quitButton.SetActive(true);
            resumeButton.SetActive(true);            
        }
    }



    public void ResumeGame()
    {
        modal.SetActive(false);
        quitButton.SetActive(false);
        resumeButton.SetActive(false);
    }



    public void LeaveGame()
    {
        modal.SetActive(false);
        PhotonNetwork.Disconnect();
        GameObject networkManager = GameObject.Find("PhotonNetworkManager");
        Destroy(networkManager);   
        SceneManager.LoadScene("Menu");
    }
}
