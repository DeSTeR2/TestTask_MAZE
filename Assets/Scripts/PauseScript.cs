using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour // NEEDS TO OPERATE WITH PAUSE AND GAME EXIT
{

    public GameObject pauseMenu, player;

    public void popPause() {
        player.GetComponent<PlayerScript>().isPause = true; // check if pause button was clicked so pop up pause menu
        pauseMenu.SetActive(true); // poping up pause menu
    }
    public void resumeApp() {
        player.GetComponent<PlayerScript>().isPause = false; // if pause button wasn`t clicked so pop down pause menu
        pauseMenu.SetActive(false); // poping down
    }
    public void exitApp() {
        Application.Quit(); // close the game
    }

}
