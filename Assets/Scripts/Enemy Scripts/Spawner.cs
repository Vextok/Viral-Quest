using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    public GameObject red_virus;
    public GameObject blue_virus;
    //public GameObject zombie_2;
    float span = 4.0f;
    float delta = 0f;


    private List<GameObject> activeVirus = new List<GameObject>();
    private int maxVirus = 5;
    private System.Random randomVirus = new System.Random();

    public float spawnWidth = 5.0f;
    public float redspawnLowerBoundHeight = 0.5f;
    public float redspawnUpperBoundHeight = 0.5f;
    public float bluespawnHeightLowerBound = -4.0f;
    public float blueSpawnHeightUpperBound = 0.0f;
    private Vector3 spawnPosition;
    
    public LayerMask exclude;
    public bool VentOpen = true;
    public bool isClosed = false;
    public Animator animator;
    private float safeDistanceFromPlayer = 3.0f;

    


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if( VentOpen && activeVirus.Count < maxVirus){
          this.delta += Time.deltaTime;
        if (this.delta > this.span)

        {
            this.delta = 0f;
            int randomNum = randomVirus.Next(1,3);
            GameObject go = (randomNum == 1) ? Instantiate(red_virus) : Instantiate(blue_virus);
            bool isValidPosition = false;
            int spawnAttempt = 0;

            while (!isValidPosition && spawnAttempt < 10){
            float randomX = Random.Range(-spawnWidth, spawnWidth);
            float randomY = (randomNum ==1) ? 
            Random.Range(redspawnLowerBoundHeight, redspawnUpperBoundHeight) : 
            Random.Range(bluespawnHeightLowerBound,blueSpawnHeightUpperBound);

            spawnPosition = new Vector3(this.transform.localPosition.x + randomX, 
            this.transform.localPosition.y + randomY, this.transform.position.z);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 playerPosition = player.transform.position;
            //Debug.Log($"Spawn Position: {spawnPosition}, Player Position: {playerPosition}, Safe Distance: {safeDistanceFromPlayer}");
           // Debug.Log($"randomX: {randomX}, randomY: {randomY}, Spawn Position: {spawnPosition}");
            //Before I had several distinct boolean logic statements checking isValidPositions, which are all now checked 
            //seperately and are all checked to be true or false with an and statement for isValidPosition.
            bool PlayerSafe = Vector3.Distance(spawnPosition, playerPosition) >= safeDistanceFromPlayer;
          

            bool noOverlapStatic = !Physics2D.OverlapCircle(spawnPosition, 0.5f, LayerMask.GetMask("Enemy"));

            bool noOverlapActive = true;
                foreach (GameObject enemy in activeVirus){
                    float enemyY = enemy.transform.position.y;
                    float enemyX = enemy.transform.position.x;
                    if(Mathf.Abs(spawnPosition.y - enemyY) < 0.5f || Mathf.Abs(spawnPosition.x - enemyX) < 0.5f){
                        noOverlapActive = false;
                        break;
                    }
                }
            
            isValidPosition = PlayerSafe && noOverlapStatic && noOverlapActive;
            spawnAttempt++;
            }

            if(isValidPosition){
            go.transform.position = spawnPosition;
            activeVirus.Add(go);
            Virus virusScript = go.GetComponent<Virus>();
                if (virusScript != null)
                {
                    virusScript.OnDeath += HandleVirusDeath; 
                }
            }

            else{
                Destroy(go);
            }
            
        
           
        }
    }

    if(!VentOpen){
        isClosed = !isClosed;
        animator.SetBool("isClosed", isClosed);
    }
    }

    private void HandleVirusDeath(GameObject virus){
        if(virus != null){
        activeVirus.Remove(virus);
        }
    }
}

