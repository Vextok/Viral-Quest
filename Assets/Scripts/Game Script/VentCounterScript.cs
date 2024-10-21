using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentCounterScript : MonoBehaviour
{

    private int closedVentCounter = 0, threshold;
    public GameObject bossLevel, nextLevel;
    private SceneEndingScript boss, next;
    public RadioScript radioScript; 
    // Start is called before the first frame update
    void Start()
    {
        boss = bossLevel.TryGetComponent(out SceneEndingScript ses) ? ses : null;
        next = nextLevel.TryGetComponent(out SceneEndingScript ses2) ? ses2 : null;
        if (boss == next) threshold = -1; else threshold = 9;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClosedVent(){
        closedVentCounter++;
        DataCollector.tempVents++;
        Debug.Log("Total Closed Vents: " + closedVentCounter);
        if(closedVentCounter == threshold){
            if(boss!=null){
            boss.EnableTrigger();
            radioScript.ActiveRadio(RadioScript.RadioState.Boss);
            }
        }

        if(closedVentCounter == 5){
            if(next!=null){
            next.EnableTrigger();
            radioScript.ActiveRadio(RadioScript.RadioState.Shop);
            }
        }
    }
}
