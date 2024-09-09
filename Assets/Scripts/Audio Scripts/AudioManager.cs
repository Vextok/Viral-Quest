using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource, sfxSource;

    public AudioClip background;
    public AudioClip death;
    public AudioClip walking;
    public AudioClip jumping;
    public AudioClip level1;

    private void Start()
    {
        // If the scene is the shop or vent
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            // Code for looping Grom's shop theme
            musicSource.clip = background;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    public void playSFX(AudioClip soundEffect)
    {
        sfxSource.PlayOneShot(soundEffect);
    }
}
