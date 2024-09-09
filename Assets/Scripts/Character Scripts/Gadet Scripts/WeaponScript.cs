using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour {
    public Transform firePoint;
    public Transform playerPoint;

    //Prefabs for Red Spray
    public GameObject Red_bulletPrefab;
    public GameObject Red_conePrefab;
    public GameObject Red_sprayBottlePrefab;

    //Prefabs for Blue Spray
    public GameObject Blue_bulletPrefab;
    public GameObject Blue_conePrefab;
    public GameObject Blue_sprayBottlePrefab;


    private GameObject activeSprayBottle;
    public enum NozzleType{Ranged, Close}
    public NozzleType currentNozzle = NozzleType.Ranged;
    private float cooldown_timer = 1000f;
    private float lastShotTime = 0f;

    private int currentWeapon = 1;
    public Text NozzleText;
    // Update is called once per frame
    void Update() {
        // Press E key to toggle which spray is active
        if (Input.GetKeyDown(KeyCode.E)) {
            swapNozzle();
        }
        // Press J key to spray
        if (Input.GetButtonDown("Fire1")) {
             if (Time.time * 1000f >= lastShotTime + cooldown_timer) {
                Shoot();
                lastShotTime = Time.time * 1000f;  // Update the last shot time
                //Debug.Log("Shot fired at: " + Time.time * 1000f);
            } else {
                //Debug.Log("Cannot shoot yet! Wait for cooldown. Next available shot at: " + (lastShotTime + cooldown_timer));
            }

            
            
        }

        if(Input.GetKeyDown(KeyCode.Q)){
            SwapWeapon();
        }
        
    }

    
    // If Ranged, change to close, otherwise go back to ranged
    void swapNozzle() {
        if (currentNozzle == NozzleType.Ranged) {
            currentNozzle = NozzleType.Close;
        }
        else {
            currentNozzle = NozzleType.Ranged;
        }
        UpdateNozzleType();
        
    }


    void SwapWeapon() {
    if(currentWeapon == 1) {
        currentWeapon = 2;
    } else {
        currentWeapon = 1;
    }
    UpdateNozzleType();
}

    void Shoot() {

        GameObject currentBullet = currentWeapon == 1 ? Red_bulletPrefab : Blue_bulletPrefab;
        GameObject currentCone = currentWeapon == 1 ? Red_conePrefab : Blue_conePrefab;
        GameObject currentBottle = currentWeapon == 1 ? Red_sprayBottlePrefab : Blue_sprayBottlePrefab;

        // If ranged, the player will shoot long range spray from the firepoint
        if(currentNozzle == NozzleType.Ranged) {
            Instantiate(currentBullet, firePoint.position, firePoint.rotation);
        }
        // Otherwise the player will shoot a short ranged cone from the firepoint
        else {
            Vector3 direction = firePoint.right;
           // Debug.Log("FirePoint Forward Direction: " + direction); // Log direction
            float offsetDist = 1f;
            Vector3 offset = direction * offsetDist;
            Vector3 adjustPos = firePoint.position + offset;
           // Debug.Log("Adjusted Position Before Z: " + adjustPos);
            adjustPos.z = 4.9f;
           // Debug.Log("Adjusted Position: " + adjustPos);
            Instantiate(currentCone, adjustPos, firePoint.rotation);
        }


        if (currentBottle != null){
                if (activeSprayBottle == null){
                    activeSprayBottle = Instantiate(currentBottle, firePoint.position, firePoint.rotation);
                    activeSprayBottle.transform.parent = firePoint;

                    Animator animator = activeSprayBottle.GetComponent<Animator>();
                    if(animator!=null){
                        //animator.SetTrigger("Spray Bottle");
                    }

                }
                StartCoroutine(HideSpray());
            }
    }

    IEnumerator HideSpray(){
        yield return new WaitForSeconds(0.2f);
        if (activeSprayBottle != null){
            Destroy(activeSprayBottle);
        }
    }

    void UpdateNozzleType(){
        if(currentWeapon == 1){
            NozzleText.color = Color.red;
        }
        else{
            NozzleText.color = Color.blue;
        }
        if(currentNozzle == NozzleType.Ranged){
            NozzleText.text = "Long Range";
        }
        else{
            NozzleText.text = "Close Range";
        }
    
    }

    
}
