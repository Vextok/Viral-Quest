using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesScript : MonoBehaviour {
    public List<Image> lifeImages;  
    public Sprite fullLifeSprite;   
    


    // Call this function to update lives
    public void UpdateLives(int livesRemaining) {
        for (int i = 0; i < lifeImages.Count; i++) {
            if (i < livesRemaining) {
                lifeImages[i].sprite = fullLifeSprite;  
                lifeImages[i].enabled = true;
            } else {
                lifeImages[i].enabled = false; 
            }
        }
    }
}
