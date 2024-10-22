using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

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
            }
        }
        else if (other.CompareTag("Boss"))
            if (other.TryGetComponent(out Boss boss))
            {
                boss.Disable();
            }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Virus"))
        {
            if (other.TryGetComponent(out Virus virus))
            {
                virus.OutCloud();
            }
        }
        else if (other.CompareTag("Boss"))
            if (other.TryGetComponent(out Boss boss))
            {
                boss.Enable();
            }
    }
}
