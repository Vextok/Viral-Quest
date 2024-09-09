
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneControllerScript : MonoBehaviour
{
    public static SceneControllerScript instance;
    AudioManager audioManagerScript;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        GameObject audioManager = GameObject.FindGameObjectWithTag("Audio");
        audioManagerScript = audioManager.GetComponent<AudioManager>();
    }
    public void NextLevel()
    {
        // Load scene 2, the vent
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        if (SceneManager.GetActiveScene().buildIndex + 1 == 3)
        {
            audioManagerScript.musicSource.Stop();
            audioManagerScript.musicSource.clip = audioManagerScript.level1;
            audioManagerScript.musicSource.loop = true;
            audioManagerScript.musicSource.Play();
        }
    }

    public void LoadScene (string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
