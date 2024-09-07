using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentEndingScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            // Go to Next Level
            SceneControllerScript.instance.NextLevel();
        }

    }
}
