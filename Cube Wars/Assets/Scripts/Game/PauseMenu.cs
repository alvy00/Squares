using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseMenuButton;


   public static bool gameIsPaused = false;
   private bool pressedPause = false;


    void Update()
    {
        if(pressedPause)
        {
            Pause();
        }
    }

    public void OnClickPauseMenuUI()
    {
        pressedPause = true;
    }

    public void Resume()
    {
        pauseMenuButton.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pressedPause = false;
        gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuButton.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
