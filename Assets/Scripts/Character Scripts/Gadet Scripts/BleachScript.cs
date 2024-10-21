using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleachScript : MonoBehaviour
{

    BossHealth bossDmgScript;
    private bool dmgDealt = false;
    Virus virusScript;
    // Start is called before the first frame update
    void Start()
    {
        //bossDmgScript = GameObject.FindWithTag("Boss").GetComponent<BossHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(dmgDealt){
            return;
        }
        if(other.CompareTag("Virus")){
            virusScript = other.GetComponent<Virus>();
            DataCollector.tempPoints += 50;
            virusScript.HandleDeath();
            //Destroy(gameObject);
        }
        if(other.CompareTag("Boss")){
            List<float> bleachdmg = new() {20f,20f,20f};
            bossDmgScript = other.GetComponent<BossHealth>();
            bossDmgScript.TakeDamage(bleachdmg);
            dmgDealt = true;
            //Destroy(gameObject);
            //Destroy(collidedWith);
        }
    }

}
