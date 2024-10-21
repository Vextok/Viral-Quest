using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GromShopScript : MonoBehaviour
{
    public GameObject shopMenu;
    static private bool RedBottleBought = false;
    static private bool BlueBottleBought = false;
    public GromShopLevelSelect ventEnding;
    
    public void RedBottle(){
        RedBottleBought = true;
        CheckItems();
    }

    public void BlueBottle(){
        BlueBottleBought = true;
        CheckItems();
    }

    private void CheckItems(){
        if(RedBottleBought && BlueBottleBought){
            ventEnding.EnableLevelSelect();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Enter Shop
            shopMenu.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Exit Shop
            shopMenu.SetActive(false);
        }
    }
}
