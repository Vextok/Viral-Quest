using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodEndScript : MonoBehaviour
{
    AudioManager audioManagerScript;
    // Start is called before the first frame update
   void Start()
    {
        GameObject audioManager = GameObject.FindGameObjectWithTag("Audio");
       audioManagerScript = audioManager.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //audioManagerScript.musicSource.Stop();
        //If user presses mouse 1 (left click), then the game screen is loaded
       if (Input.GetKeyDown(KeyCode.Space))
       {
        SceneManager.LoadScene("Main Menu");
       } 
    }
}
