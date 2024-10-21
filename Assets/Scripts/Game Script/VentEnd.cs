using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VentEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            DataCollector.ResetLevelScores();
            // Go to Next Level
            SceneControllerScript.instance.NextLevel();
        }
    }
}
