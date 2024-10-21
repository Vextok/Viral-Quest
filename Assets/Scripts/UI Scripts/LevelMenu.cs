using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;
    public static int unlockedLevel = 1;

    private void Awake(){
       for(int i = 0; i < buttons.Length; i++){
        buttons[i].interactable = false;
       }
       for(int i = 0; i < unlockedLevel; i++){
            buttons[i].interactable = true;
       }
       if(!BossHealth.bossDefeated){
        buttons[2].interactable = false;
       }
    }
    public void OpenLevel(int levelId){
        SceneManager.LoadScene(levelId);
    }
}
