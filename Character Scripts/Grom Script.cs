using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GromScript : MonoBehaviour
{
    public Transform player;
    public float range = 10f;
    public LayerMask playerLayer;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        LoS();
    }

    private void LoS(){
        Vector2 direction = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, playerLayer);
        Debug.DrawRay(transform.position, direction * range, Color.red);
        if(hit.collider != null && hit.collider.transform == player){
            animator.SetBool("CanSeePlayer", true);
        }
        else{
            animator.SetBool("CanSeePlayer", false);
        }
    }
}
