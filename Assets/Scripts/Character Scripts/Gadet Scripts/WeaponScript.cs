using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

    //Prefabs for Yellow Spray
    public GameObject Yellow_bulletPrefab;
    public GameObject Yellow_conePrefab;
    public GameObject Yellow_sprayBottlePrefab;

    //Prefabs and Variables for Grenade
    public GameObject grenadePrefab;
    public float throwForce = 5f;
    public float explosionDelay = 2f;
    public float cloudGrowthDuration = 3f;  
    private float maxCloudSize = 6f;  
    public GameObject cloudPrefab;

    public GameObject bleachPrefab;


    private GameObject activeSprayBottle;
    public enum NozzleType{Ranged, Close}
    public NozzleType currentNozzle = NozzleType.Ranged;
    private float cooldown_timer = 700f;
    private float lastShotTime = 0f;

    private int currentWeapon = 1;
    //public Text NozzleText;
    public AudioClip longRange;
    public AudioClip closeRange;
    public AudioSource audioSource;

    public Image weaponType;
    public Sprite LongRangeRed;
    public Sprite CloseRangeRed;

    public Sprite LongRangeBlue;
    public Sprite CloseRangeBlue;

    public Sprite LongRangeYellow;
    public Sprite CloseRangeYellow;

    public Sprite grenadeSprite;

    public Sprite bleachSprite;


    public Image beltType;
    public Sprite redSelected;
    public Sprite blueSelected;
    public Sprite yellowSelected;
    public Sprite GrenadeOrBleachSelected; 

    private float lastSwap;
    private bool BeltShown = false;
    private float hideDelay = 1.0f;

    public PlayerMovement playerMovement;

    private int totalBleachUses = 0;
    private int totalGrenadeUses = 0;
    private int maxUse = 3;

    public GameObject noneLeft;


    void Start(){
        Color temp = beltType.color;
        temp.a = 0f;
        beltType.color = temp;
    }


    // Update is called once per frame
    void Update() {

        if(BeltShown && Time.time >= lastSwap + hideDelay){
            Color temp = beltType.color;
            temp.a = 0f;
            beltType.color = temp;
            //BeltShown = false;
        }
        // Press E key to toggle which spray is active
        if (Input.GetKeyDown(KeyCode.X)) {
            swapNozzle();
        }
        // Press J key to spray
        if (Input.GetKeyDown(KeyCode.Z) && SceneManager.GetActiveScene().buildIndex != 1) {
             if (Time.time * 1000f >= lastShotTime + cooldown_timer) {
                Shoot();
                lastShotTime = Time.time * 1000f;  // Update the last shot time
                //Debug.Log("Shot fired at: " + Time.time * 1000f);
                
             }
             else {
                //Debug.Log("Cannot shoot yet! Wait for cooldown. Next available shot at: " + (lastShotTime + cooldown_timer));
             }
        }

        if(Input.GetKeyDown(KeyCode.C)){
            SwapWeaponForward();
        }

        if(Input.GetKeyDown(KeyCode.V)){
            SwapWeaponBackward();
        }
        
    }

    
    // If Ranged, change to close, otherwise go back to ranged
    void swapNozzle() {
        if (currentNozzle == NozzleType.Ranged) {
            currentNozzle = NozzleType.Close;
            cooldown_timer /= 3f;                                           //Sean
        }
        else{
            currentNozzle = NozzleType.Ranged;
            cooldown_timer *= 3f;                                           //Sean
        }
        UpdateNozzleType();
        
    }


    void SwapWeaponForward() {
        lastSwap = Time.time;
    if(currentWeapon == 1) {
        currentWeapon = 2;
    } else if (currentWeapon == 2){
        currentWeapon = 3;
    }
    else if (currentWeapon == 3){
        currentWeapon = 4;
    }
    else if (currentWeapon == 4){
        currentWeapon = 5;
    }
    else{
        currentWeapon = 1;
    }
    UpdateBeltType();
    UpdateNozzleType();
}

void SwapWeaponBackward() {
        lastSwap = Time.time;
    if(currentWeapon == 1) {
        currentWeapon = 5;
    } else if (currentWeapon == 5){
        currentWeapon = 4;
    }
    else if (currentWeapon == 4){
        currentWeapon = 3;
    }
    else if (currentWeapon == 3){
        currentWeapon = 2;
    }
    else{
        currentWeapon = 1;
    }
    UpdateBeltType();
    UpdateNozzleType();
}

    void Shoot() {

        GameObject currentBullet = null;
        GameObject currentCone = null;
        GameObject currentBottle = null;

        switch(currentWeapon){
        case 1:
            currentBullet = Red_bulletPrefab;
            currentCone = Red_conePrefab;
            currentBottle = Red_sprayBottlePrefab;
            break;
        case 2:
            currentBullet = Blue_bulletPrefab;
            currentCone = Blue_conePrefab;
            currentBottle = Blue_sprayBottlePrefab;
            break;
        case 3:
            currentBullet = Yellow_bulletPrefab;
            currentCone = Yellow_conePrefab;
            currentBottle = Yellow_sprayBottlePrefab;
            break;

        case 4:
            ThrowGrenade();
            break;

        case 5:
            BleachPour();
            break;

        default:
            return;
        }

        // If ranged, the player will shoot long range spray from the firepoint
        if(currentBullet != null && currentCone != null){
        if(currentNozzle == NozzleType.Ranged) {
            Instantiate(currentBullet, firePoint.position, firePoint.rotation);
            PlaySound(longRange, 1f);
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
            PlaySound(closeRange, 1f);
        }
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
        yield return new WaitForSeconds(cooldown_timer / 1333f);              //This is also Sean messing with subtle things.
        if (activeSprayBottle != null){
            Destroy(activeSprayBottle);
        }
    }

    void UpdateBeltType(){
        if(currentWeapon == 1){
            //weaponType.sprite = (currentNozzle == NozzleType.Ranged) ? LongRangeRed : CloseRangeRed;
            beltType.sprite = redSelected;
            Color temp = beltType.color;
            temp.a = 1f;
            beltType.color = temp;
            BeltShown = true;
        }
        else if (currentWeapon == 2){
            //weaponType.sprite = (currentNozzle == NozzleType.Ranged) ? LongRangeBlue : CloseRangeBlue;
            beltType.sprite = blueSelected;
            Color temp = beltType.color;
            temp.a = 1f;
            beltType.color = temp;
            BeltShown = true;
        }
        else if (currentWeapon == 3){
            //weaponType.sprite = (currentNozzle == NozzleType.Ranged) ? LongRangeYellow : CloseRangeYellow;
            beltType.sprite = yellowSelected;
            Color temp = beltType.color;
            temp.a = 1f;
            beltType.color = temp;
            BeltShown = true;
        }
        else if (currentWeapon == 4 || currentWeapon == 5){
           // weaponType.sprite = grenadeSprite;
            beltType.sprite = GrenadeOrBleachSelected;
            Color temp = beltType.color;
            temp.a = 1f;
            beltType.color = temp;
            BeltShown = true;
        }
       //else if (currentWeapon == 5){
            //weaponType.sprite = bleachSprite;
       // }
    
    }

    void UpdateNozzleType(){
        if(currentWeapon == 1){
            weaponType.sprite = (currentNozzle == NozzleType.Ranged) ? LongRangeRed : CloseRangeRed;
        }
        else if (currentWeapon == 2){
            weaponType.sprite = (currentNozzle == NozzleType.Ranged) ? LongRangeBlue : CloseRangeBlue;
        }
        else if (currentWeapon == 3){
            weaponType.sprite = (currentNozzle == NozzleType.Ranged) ? LongRangeYellow : CloseRangeYellow;
        }
        else if (currentWeapon == 4){
            weaponType.sprite = grenadeSprite;
        }
        else if (currentWeapon == 5){
            weaponType.sprite = bleachSprite;
        }
    
    }

    void PlaySound(AudioClip clip, float volume){
        if (clip != null) {
        audioSource.PlayOneShot(clip, volume);
    
    }
    }

    void ThrowGrenade(){

        if(totalGrenadeUses >= maxUse){
            noneLeft.SetActive(true);
            StartCoroutine(noneDuration());
            return;
        }
        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        if(rb!=null){
            Vector2 throwDirection = firePoint.right;
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }
        totalGrenadeUses++;
        StartCoroutine(GrenadeExplosion(grenade));
    }

    IEnumerator GrenadeExplosion(GameObject grenade){
        yield return new WaitForSeconds(explosionDelay);

        float yOffset = 1.0f; 

        
        Vector3 cloudPosition = new Vector3(grenade.transform.position.x, grenade.transform.position.y + yOffset, grenade.transform.position.z);
        GameObject cloud = Instantiate(cloudPrefab, cloudPosition, Quaternion.identity);
    
        Destroy(grenade);

        StartCoroutine(CloudGrowth(cloud));
    }

    IEnumerator CloudGrowth(GameObject cloud){
        CircleCollider2D cloudCollider = cloud.GetComponent<CircleCollider2D>();
        float startSize = 1f;

        float elapsedTime = 0f;
        Vector3 initialScale = cloud.transform.localScale;
        float originalRadius = cloudCollider.radius;

        while (elapsedTime < cloudGrowthDuration) {
        elapsedTime += Time.deltaTime;

       
        float newSize = Mathf.Lerp(startSize, maxCloudSize, elapsedTime / cloudGrowthDuration);
        cloud.transform.localScale = new Vector3(newSize, newSize, 1f);
        if (cloudCollider != null) {
            cloudCollider.radius = (newSize / initialScale.x) * originalRadius;
        }

        yield return null;
    }

    yield return new WaitForSeconds(2.9f);  
    Destroy(cloud);

    }

    void BleachPour(){

        if(totalBleachUses >= maxUse){
            noneLeft.SetActive(true);
            StartCoroutine(noneDuration());
            return;
        }
        //Need to add a slight offset in the X direction to adjust the bleach Instantiating spot.
        //Also, freeze player movement when using bleach. 
        if(!playerMovement.grounded){
            return;
        }
        playerMovement.FreezeMovement(true);
        Vector3 offset = firePoint.right * 0.5f + firePoint.up * -0.02f;
        GameObject bleach = Instantiate(bleachPrefab, firePoint.position + offset, firePoint.rotation);
        totalBleachUses++;
        StartCoroutine(BleachDuration(bleach));
    }

    IEnumerator BleachDuration(GameObject bleach){
        yield return new WaitForSeconds(1.5f);
        playerMovement.FreezeMovement(false);
        Destroy(bleach);
    }

    IEnumerator noneDuration(){
        yield return new WaitForSeconds(2.0f);
        noneLeft.SetActive(false);
    }

    /*IEnumerator BleachAreaPuddle(GameObject bleach){
        pourCollider = bleach.GetComponent<BoxCollider2D>();
        areaCollider = bleach.GetComponent<BoxCollider2D>();

        pourCollider.enabled = true;

        yield return StartCoroutine(DownwardPour());

        areaCollider.enabled = true;

        yield return StartCoroutine(PuddleArea());

        Tried to get fancy with the bleach colliders but didn't go well. 

    }*/
    
    
}
