using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1EndingScript : MonoBehaviour
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
            SceneControllerScript.instance.NextLevel();
        }

    }

    public void DisableTrigger(){
        triggerArea.enabled = false;
    }

    public void EnableTrigger(){
        triggerArea.enabled = true;
    }
}
