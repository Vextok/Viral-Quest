using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{

    public int lives = 3;
    public LivesScript livesUI;
    private float iFrames = 2f;
    private bool isImmune = false;
    public AudioSource audioSource;
    public AudioClip livesLost;
    public PlayerMovement player;
    public AudioClip OneLifeLost;
    public GameObject gameOverUI;
    //public GameObject pauseM;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLivesScript();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveLife(){
        if(lives > 0 && !isImmune){
            lives--;
            audioSource.PlayOneShot(OneLifeLost, 2.0f);
            UpdateLivesScript();
            //audioSource.PlayOneShot(OneLifeLost);
            //Debug.Log("Life removed. Lives remaining: " + lives);
            if (lives <= 0){
                StartCoroutine(ResetLevel());
            }
            else{
            StartCoroutine(ApplyIFrames());
            }
        }
    }

    private void UpdateLivesScript(){
        if(livesUI != null){
            livesUI.UpdateLives(lives);
        }
    }

    private IEnumerator ApplyIFrames(){
        isImmune = true;
        //Debug.Log("Immunity activated. Player is immune for " + iFrames + " seconds.");
        yield return new WaitForSeconds(iFrames);
        isImmune = false;
        //Debug.Log("Immunity ended. Player is no longer immune.");
    }

    private IEnumerator ResetLevel(){
        if(player != null){
            player.FreezeMovement(true);
        }
        if (audioSource != null && livesLost != null){
            audioSource.PlayOneShot(livesLost);
            gameOverUI.SetActive(true);
            yield return new WaitForSeconds(livesLost.length);
            gameOverUI.SetActive(false);
        }
        gameOverUI.SetActive(true);
        yield return new WaitForSeconds(livesLost.length);
        gameOverUI.SetActive(false);
        DataCollector.ResetLevelScores();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //pauseM.SetActive(true);
        //pauseM.GetComponent<PauseMenu>().Restart();
    }
}
