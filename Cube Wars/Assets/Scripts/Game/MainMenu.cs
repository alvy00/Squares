using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject optionsScreen;
    public GameObject creditsScreen;


    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CreditsButton()
    {
        menuScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void OptionsButton()
    {
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void CreditsBackButton()
    {
        creditsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void OptionsBackButton()
    {
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }
}
