using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource, sfxSource;

    public AudioClip main_menu;
    public AudioClip background;
    public AudioClip death;
    public AudioClip walking;
    public AudioClip jumping;
    public AudioClip level1;
    public AudioClip BossTheme;
    public AudioClip StageClear;
    public AudioClip level2;
    public AudioClip GoodEnd;
    public AudioClip BadEnd;
    //public AudioClip virusDeath;
    //public AudioClip bossDeath;

    private void Start()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;

        
        PlayMusicForScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.buildIndex);
    }

    private void PlayMusicForScene(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 0:
                // Play main menu music
                musicSource.clip = main_menu;
                musicSource.loop = true;
                break;
            case 1:
            case 2:
                // Play background music
                musicSource.clip = background;
                musicSource.loop = true;
                break;

            case 3:
                musicSource.clip = level1;
                musicSource.loop = true;
                break;
            case 4:
                musicSource.clip = BossTheme;
                musicSource.loop = true;
                break;

            case 5:
                musicSource.clip = GoodEnd;
                musicSource.loop = true;
                break;

            case 6:
                musicSource.clip = level2;
                musicSource.loop = true;
                break;

            case 7:
                musicSource.clip = BossTheme;
                musicSource.loop = true;
                break;

            case 8:
                musicSource.clip = BadEnd;
                musicSource.loop = true;
                break;
            default:
                break;
        }

        musicSource.Play();
    }

    public void playSFX(AudioClip soundEffect)
    {
        sfxSource.PlayOneShot(soundEffect);
    }

    public void playStageClear(AudioClip clear){
        musicSource.clip = clear;
        musicSource.loop = false;
        musicSource.Play();
    }
}
