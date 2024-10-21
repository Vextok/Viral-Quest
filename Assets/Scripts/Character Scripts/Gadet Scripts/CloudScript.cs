using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    private List<Virus> tempViruses;
    private Boss tempBoss;

    // Start is called before the first frame update
    void Start()
    {
        tempViruses = new();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Virus"))
        {
            if (other.TryGetComponent(out Virus virus))
            {
                virus.CloudSlow(0.7f);
                tempViruses.Add(virus);
            }
        }
        else if (other.CompareTag("Boss"))
            if (other.TryGetComponent(out Boss boss))
            {
                boss.Disable();
                tempBoss = boss;
            }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Virus"))
        {
            if (other.TryGetComponent(out Virus virus))
            {
                virus.OutCloud();
                tempViruses.Remove(virus);
            }
        }
        else if (other.CompareTag("Boss"))
            if (other.TryGetComponent(out Boss boss))
            {
                boss.Enable();
            }
    }
    private void OnDestroy()//This is for when the cloud is destroyed. The viruses are still slow and Boss2 is still disabled, otherwise.
    {
        //foreach (Virus v in tempViruses)
        //    v.OutCloud();
        //if(tempBoss) tempBoss.Enable();
    }
}
