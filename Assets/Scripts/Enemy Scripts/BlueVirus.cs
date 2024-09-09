using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueVirus : MonoBehaviour
{
   
    //float time = 0.0f;
    // Start is called before the first frame update
    private bool isDead = false;
    private float knockback = 2.0f;
    private Rigidbody2D rb;
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
        
    }

    void OnCollisionEnter2D (Collision2D coll){
        GameObject collidedWith = coll.gameObject;
       
        if(collidedWith.tag  == "Red Spray Bullet"){
            Destroy(collidedWith);
            
        }
        if(collidedWith.tag  == "Red Spray Cone"){
            ApplyKnockback(coll);
            //Destroy(collidedWith);
            
        }
         if(collidedWith.tag  == "Blue Spray Bullet"){
            Destroy(collidedWith);
            isDead = true;
        }
        if(collidedWith.tag  == "Blue Spray Cone"){
            //Destroy(collidedWith);
            isDead = true;
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
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
