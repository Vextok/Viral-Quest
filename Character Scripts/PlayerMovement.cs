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
    public LayerMask groundMask;
    public Animator animator;
    public HealthManager healthManager;
    private bool isFrozen = false;
    public GameObject hitMarker;
    public GameObject PauseMenu;
    PauseMenu pauseMenuScript;
    AudioManager audioManagerScript;
    //public AudioSource walkSource;
    //public AudioClip walk;

    // Start is called before the first frame update
    void Start() {
        GameObject audioManager = GameObject.FindGameObjectWithTag("Audio");
        pauseMenuScript = PauseMenu.GetComponent<PauseMenu>();
        audioManagerScript = audioManager.GetComponent<AudioManager>();
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
        if(!isFrozen){
        xInput = Input.GetAxis("Horizontal");
    }
    }

    void MoveWithInput() {
        // If there is ANY x input...
        if (Mathf.Abs(xInput) > 0) {

            // Change the x velocity
            //walkAudio();
            increment = xInput * acceleration;
            newSpeed = Mathf.Clamp(body.velocity.x + increment, -groundSpeed, groundSpeed);
            body.velocity = new Vector2(newSpeed, body.velocity.y);

            //Set xVelocity condition to xInput
            animator.SetFloat("xVelocity", Mathf.Abs(xInput));

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
            animator.SetFloat("xVelocity", Mathf.Abs(xInput));
        }
       
    }
   /* IEnumerator walkAudio(){
         walkSource.clip = walk;
         walkSource.Play();
         yield return new WaitForSeconds(1);

    }*/
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

