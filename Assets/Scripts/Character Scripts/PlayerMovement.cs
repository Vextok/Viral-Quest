using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public float groundSpeed, jumpSpeed, acceleration;
    [Range(0f, 1f)]
    public float groundDecay;
    float xInput, yInput, direction, increment, newSpeed;
    public bool grounded;
    public bool facingLeft;
    public BoxCollider2D groundCheck;
    private LayerMask groundMask;               //Made this private, again cuz it's working, now.
    public Animator animator;
    public HealthManager healthManager;
    private bool isFrozen = false;
    public GameObject hitMarker;
    public GameObject PauseMenu;
    PauseMenu pauseMenuScript;
    AudioManager audioManagerScript;
    public AudioSource walkSource;
    public AudioClip walk;

    // Start is called before the first frame update
    void Start() {
        GameObject audioManager = GameObject.FindGameObjectWithTag("Audio");
        pauseMenuScript = PauseMenu.GetComponent<PauseMenu>();
        audioManagerScript = audioManager.GetComponent<AudioManager>();
        groundMask = 1001 << GameObject.Find("Ground - Tile Map").layer;        //This is why.
        //Boss level has platforms that the boss can pass through, but the player can't.
    }
    // Update is called once per frame
    void Update() {
        pauseMenuScript.PauseEscape();
        // Move the player
        if(!isFrozen){
        GetInput();
        HandleJump();
        }
    }
    private void FixedUpdate() {
        if(!isFrozen){
        MoveWithInput();
        CheckGround();
        ApplyFriction();
        }
    }

    void GetInput() {
        // Get X and from A and D and Left and Right keys
        if (!isFrozen) {
        // Check for LeftArrow or RightArrow key presses
        if (Input.GetKey(KeyCode.LeftArrow)) {
            xInput = -1f;  // Move left
        } 
        else if (Input.GetKey(KeyCode.RightArrow)) {
            xInput = 1f;   // Move right
        } 
        else {
            xInput = 0f;   // No movement
        }
    }
    }

    void MoveWithInput() {
        // If there is ANY x input...
        if (Mathf.Abs(xInput) > 0) {

            // Change the x velocity
            PlayWalkAudio();
            increment = xInput * acceleration;
            newSpeed = Mathf.Clamp(body.velocity.x + increment, -groundSpeed, groundSpeed);
            body.velocity = new Vector2(newSpeed, body.velocity.y);

            //Set xVelocity condition to xInput
            animator.SetFloat("xVelocity", Mathf.Abs(xInput));
            animator.SetFloat("yVelocity", yInput);

            // Check if the Player Should Flip
            direction = Mathf.Sign(xInput);
            if (direction < 0 && !facingLeft)
            {
                Flip();
            }
            else if (direction > 0 && facingLeft)
            {
                Flip();
            }
        }
        // If no xInput, set condition for idle animation
        else if (Mathf.Abs(xInput) == 0)
        {
            StopWalkAudio();
            animator.SetFloat("xVelocity", Mathf.Abs(xInput));
            animator.SetFloat("yVelocity", yInput);
        }
        if(!grounded){
            StopWalkAudio();
        }
       
    }
    void PlayWalkAudio() {
    if (!walkSource.isPlaying) {
        walkSource.loop = true;
        walkSource.clip = walk;
        walkSource.Play();
    }
}

    void StopWalkAudio() {
        if (walkSource.isPlaying) {
            walkSource.loop = false;
            walkSource.Stop();
        }
    }
    void HandleJump() {
        // When the play presses spacebar on the ground, jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            audioManagerScript.playSFX(audioManagerScript.jumping);
        }
    }
    void CheckGround() {
        // Check if the player's collision check is within the groundmask
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        // Set the jumping check for animation to the opposite of grounded
        animator.SetBool("isJumping", !grounded); 
    }

    void ApplyFriction() {
        // Apply Friction as long as the player is moving and on the ground
        if ( xInput == 0 && grounded && Mathf.Abs(body.velocity.y) < 0.1f) {
            body.drag = 10f;               
        }
        else{
            body.drag = 0f;
        }
    }

    void Flip()
    {
        // When called, toggle the check
        facingLeft = !facingLeft;
        // Tranform the player object 180 degree, facing them the opposite way
        transform.Rotate(0, 180, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Virus"))
        {
            if (healthManager != null)
            {
                healthManager.RemoveLife();
                if (hitMarker != null)
            {
                Vector3 newPos = hitMarker.transform.localPosition;
                if (facingLeft)
                {
                    newPos.x = -Mathf.Abs(newPos.x);
                }
                else
                {
                    newPos.x = Mathf.Abs(newPos.x);
                }
                hitMarker.transform.localPosition = newPos;

                // Enable and show the Hit Marker
                hitMarker.SetActive(true);
                StartCoroutine(HidePoint(hitMarker));
            }
            }

        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            if (healthManager != null)
            {
                healthManager.RemoveLife();
                if (hitMarker != null)
            {
                Vector3 newPos = hitMarker.transform.localPosition;
                if (facingLeft)
                {
                    newPos.x = -Mathf.Abs(newPos.x);
                }
                else
                {
                    newPos.x = Mathf.Abs(newPos.x);
                }
                hitMarker.transform.localPosition = newPos;

                // Enable and show the Hit Marker
                hitMarker.SetActive(true);
                StartCoroutine(HidePoint(hitMarker));
            }
            }

        }
    }
        
        
    

    public void FreezeMovement(bool freeze){
        isFrozen = freeze;
        if(freeze){
            body.velocity = Vector2.zero;
            body.isKinematic = true;
        }
        else{
            body.isKinematic = false;
        }
    }

    private IEnumerator HidePoint(GameObject hitMarker){
        yield return new WaitForSeconds(2f);
        if(hitMarker != null){
        hitMarker.SetActive(false);
        }
    }
    
}

