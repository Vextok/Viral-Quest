using System.Collections;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneControllerScript : MonoBehaviour
{
    public static SceneControllerScript instance;
    AudioManager audioManagerScript;
    private string next = "";
    private int index = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
             if (instance.gameObject.activeSelf == false)
            {
                instance.gameObject.SetActive(true);  // Reactivate the original instance if it's disabled
            }

            // Destroy the duplicate GameManager
            Destroy(gameObject);
            return;
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
        if (SceneManager.GetActiveScene().buildIndex + 1 == 4)
        {
            audioManagerScript.musicSource.Stop();
            audioManagerScript.musicSource.clip = audioManagerScript.BossTheme;
            audioManagerScript.musicSource.loop = true;
            audioManagerScript.musicSource.Play();
        }
    }

    public void LoadScene (string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void LoadEndCard(string scene = "")
    {
        SceneManager.LoadScene("EndCard");
        instance.next = scene;
        instance.StartCoroutine(instance.EndCard());
    }
    public void LoadEndCard(int i = 0)
    {
        SceneManager.LoadScene("EndCard");
        instance.index = i;
        instance.StartCoroutine(instance.EndCard());
    }
    private IEnumerator EndCard()
    {
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter));
        if (next == "") SceneManager.LoadScene(index);
        else SceneManager.LoadScene(next);
    }
}
