using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusAudioScript : MonoBehaviour
{
    private Transform player;        
    public AudioSource enemyAudio;  
    public float playDistance = 5f; // The distance within which the audio will play

    private bool isPlaying = false;

    void Start(){
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.transform;
    }

    void Update()
    {
        // Calculate the distance between the player and the enemy
        float distance = Vector3.Distance(player.position, transform.position);

        
        if (distance <= playDistance && !isPlaying)
        {
            enemyAudio.Play();
            isPlaying = true;
            //Debug.Log("Playing audio for: " + gameObject.name);
        }
        
        else if (distance > playDistance && isPlaying)
        {
            enemyAudio.Stop();
            isPlaying = false;
        }
    }
}

