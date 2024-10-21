using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GromShopLevelSelect : MonoBehaviour
{
    public GameObject levelSelectScreen;
    public GameObject shopReminder;
    static private bool nextLevelScreen = false;

    public void EnableLevelSelect(){
        nextLevelScreen = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player") && nextLevelScreen)
        {
            // Go to Next Level
            //SceneControllerScript.instance.NextLevel();
            levelSelectScreen.SetActive(true);
        }

        if(collider.CompareTag("Player") && !nextLevelScreen)
        {
            // Go to Next Level
            //SceneControllerScript.instance.NextLevel();
            shopReminder.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            // Go to Next Level
            //SceneControllerScript.instance.NextLevel();
            if(levelSelectScreen !=null){
            levelSelectScreen.SetActive(false);
            }
        }

        if(collider.CompareTag("Player") && !nextLevelScreen)
        {
            // Go to Next Level
            //SceneControllerScript.instance.NextLevel();
            shopReminder.SetActive(false);
        }

    }
}
