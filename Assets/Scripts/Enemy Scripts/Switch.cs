using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // Start is called before the first frame update

    public Spawner vent;
    private bool PlayerInRange = false;
    public Animator animator;
    public bool isFlipped = false;

    public VentCounterScript ventCounter;
    private EnemyAudioManager audioManagerScript;
    void Start()
    {
        ventCounter = GameObject.Find("VentManager").GetComponent<VentCounterScript>();
        GameObject audioManager = GameObject.FindGameObjectWithTag("Enemy Audio");
        audioManagerScript = audioManager.GetComponent<EnemyAudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerInRange && Input.GetKeyDown(KeyCode.F) && vent.VentOpen){
            CloseVent();
            ventCounter.ClosedVent();
            isFlipped = !isFlipped;
            audioManagerScript.playEnemySFX(audioManagerScript.switchFlip);
            animator.SetBool("isFlipped", isFlipped);
        }
        
    }

    private void CloseVent(){
        if (vent != null){
            vent.VentOpen = !vent.VentOpen;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            PlayerInRange = false;
        }
    }
}
