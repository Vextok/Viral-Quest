using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float bulletSpeed = 20f;
    private Rigidbody2D rb;
    //float time = 0.0f;
    public float fixZ = 5f;
    private float flightTime = 0.3f;
    private float spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed;
        Vector3 startPosition = transform.position;
        startPosition.z = fixZ;
        transform.position = startPosition;
        spawnTime = Time.time;
    }

    void Update(){
        Vector3 position = transform.position;
        position.z = fixZ;
        transform.position = position;
        if(Time.time - spawnTime >= flightTime){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")){
            Destroy(gameObject);
        }
    }
}
