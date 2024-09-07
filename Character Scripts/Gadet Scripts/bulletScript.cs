using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float bulletSpeed = 20f;
    private Rigidbody2D rb;
    float time = 0.0f;
    public float fixZ = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed;
        Vector3 startPosition = transform.position;
        startPosition.z = fixZ;
        transform.position = startPosition;
    }

    void Update(){
        Vector3 position = transform.position;
        position.z = fixZ;
        transform.position = position;
        time += Time.deltaTime;
        int seconds = (int) time%60;
        if(seconds >= 1){
            time = 0;
            seconds = 0;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")){
            Destroy(gameObject);
        }
    }
}
