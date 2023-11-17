using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{

    public GameObject pauseMenu, player;

    public void popPause() {
        player.GetComponent<PlayerScript>().pause = true;
        pauseMenu.SetActive(true);
    }
    public void resumeApp() {
        player.GetComponent<PlayerScript>().pause = false;
        pauseMenu.SetActive(false);
    }
    public void exitApp() {
        Application.Quit();
    }

}
