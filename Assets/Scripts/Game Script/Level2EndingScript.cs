using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2EndingScript : MonoBehaviour
{


     private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            // Go to Next Level
            DataCollector.UpdateScores();
            SceneControllerScript.instance.NextLevel();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
