using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public static bool GameIsPaused = false;


    public void Pause(){
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
        GameIsPaused = true;
    }
    public void PauseEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
        
    }
    public void MainMenu(){
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void Resume(){
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
        GameIsPaused = false;
    }
}
