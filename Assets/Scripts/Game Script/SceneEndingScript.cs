using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEndingScript : MonoBehaviour
{
    private Collider2D triggerArea;
    private string path;
    void Start()
    {
        path = gameObject.name;
        triggerArea = GetComponent<Collider2D>();
        DisableTrigger();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
            switch(SceneManager.GetActiveScene().name)
            {
                case "Level 1": CheckPath(); break;
                case "Level 2": SceneControllerScript.instance.NextLevel(); break;
            }
        
    }
    public void DisableTrigger()
    {
        triggerArea.enabled = false;
    }
    public void EnableTrigger()
    {
        triggerArea.enabled = true;
    }
    private void CheckPath()
    {
        DataCollector.UpdateScores();
        if (path == "Boss Level" && triggerArea.enabled)
        {
            // Go to Next Level
            UnlockNewLevel();
            SceneControllerScript.instance.NextLevel();
        }
        else if(path == "Level 2" && triggerArea.enabled)
        {
            // Go to Next Level
            UnlockNewLevel();
            UnlockLevel2();
            SceneControllerScript.instance.LoadEndCard("Grom's Shop");
        }
    }

    void UnlockNewLevel(){
        if(LevelMenu.unlockedLevel == 1){
            LevelMenu.unlockedLevel++;

        }
    }

    void UnlockLevel2(){
        if(LevelMenu.unlockedLevel == 2){
            LevelMenu.unlockedLevel += 2;
        }
    }

}
