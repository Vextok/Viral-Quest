using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAudioManager : MonoBehaviour
{
    public AudioSource enemySFXSource;


    public AudioClip virusDeath;
    public AudioClip bossDeath;
    public AudioClip switchFlip;


    public void playEnemySFX(AudioClip soundEffect)
    {
        if (soundEffect == null)
    {
        Debug.LogWarning("Attempted to play a null sound effect.");
        return;
    }
        enemySFXSource.PlayOneShot(soundEffect);
    }
}
