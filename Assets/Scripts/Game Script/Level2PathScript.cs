using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2PathScript : MonoBehaviour
{
    private Collider2D triggerArea;


    void Start(){
        triggerArea = GetComponent<Collider2D>();
        DisableTrigger();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player") && triggerArea.enabled)
        {
            // Go to Next Level
            DataCollector.UpdateScores();
            SceneManager.LoadScene("Level 2");
        }

    }

    public void DisableTrigger(){
        triggerArea.enabled = false;
    }

    public void EnableTrigger(){
        triggerArea.enabled = true;
    }
}
