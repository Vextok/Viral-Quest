using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadioScript : MonoBehaviour
{

    public GameObject radioUI;
    public TextMeshProUGUI dialogueText;
    public Image dialogueImage;
    public Sprite shutters;
    public Sprite purplegrowth;

    private string returnShop = "If you're feeling sick and unwell, look for those shutter doors! They'll lead you back to the shop!";
    private string bossOpen = "I don't know what you've done, but I can feel the ground shaking from here!";

    public enum RadioState{Shop, Boss}
    private RadioState currentState;
    public Animator radioAnimator;
    public AudioSource audioSource;
    public AudioClip radioOnClip;



    public void ActiveRadio(RadioState state){
        radioUI.SetActive(true);
        audioSource.PlayOneShot(radioOnClip, 2.0f);
        radioAnimator.Play("RadioTurnsOn");
        StartCoroutine(ShowRadioDialogue(state));
    }

    IEnumerator ShowRadioDialogue(RadioState state){

        yield return new WaitForSeconds(radioAnimator.GetCurrentAnimatorStateInfo(0).length);

        switch(state){
            case RadioState.Shop:
                dialogueText.text = returnShop;
                dialogueImage.sprite = shutters;
                break;
            case RadioState.Boss:
                dialogueText.text = bossOpen;
                dialogueImage.sprite = purplegrowth;
                break;
        
        }
        currentState = state;
        radioAnimator.Play("RadioSpeaks");
        yield return new WaitForSeconds(8f);
        Deactivate();

    }


    public void Deactivate(){
        radioAnimator.Play("Radio Off");
        //radioUI.SetActive(false);
        StartCoroutine(DisableAnimation());
    }

    IEnumerator DisableAnimation(){
        yield return new WaitForSeconds(radioAnimator.GetCurrentAnimatorStateInfo(0).length);
        radioUI.SetActive(false);
    }
}
