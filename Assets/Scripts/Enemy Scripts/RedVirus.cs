using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedVirus : MonoBehaviour
{

    private bool isDead = false;
    //float time = 0.0f;
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private float knockback = 2.0f;
    private bool isGrounded = true;
    private LayerMask ground;
    private float groundDist = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead){
            Destroy(this.gameObject);
        }
        CheckGroundStatus();
        
    }

    void OnCollisionEnter2D (Collision2D coll){
        GameObject collidedWith = coll.gameObject;
       
        if(collidedWith.tag  == "Red Spray Bullet"){
            Destroy(collidedWith);
            isDead = true;
        }
        if(collidedWith.tag  == "Red Spray Cone"){
            //Destroy(collidedWith);
            isDead = true;
        }
        if(collidedWith.tag  == "Blue Spray Cone"){
            ApplyKnockback(coll);
            //Destroy(collidedWith);
            
        }
        if(collidedWith.tag  == "Blue Spray Bullet"){
            Destroy(collidedWith);
            
        }
}

void ApplyKnockback(Collision2D coll){
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        Vector2 knockbackDirect = (transform.position - coll.transform.position).normalized;
      //  Debug.Log($"Applying knockback: {knockbackDirect * knockback}");
        rb.AddForce(knockbackDirect * knockback, ForceMode2D.Impulse);
        StartCoroutine(FreezeMovement());
    }

    IEnumerator FreezeMovement(){
        yield return new WaitForSeconds(2f);
        if(isGrounded){
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

void CheckGroundStatus(){
    RaycastHit2D floor = Physics2D.Raycast(transform.position, Vector2.down, groundDist, ground);
    if(floor.collider != null){
        isGrounded = true;
        rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
    }
    else{
        isGrounded = false;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }
}
}
