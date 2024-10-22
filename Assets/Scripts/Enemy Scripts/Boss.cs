using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> viruses;

    private Rigidbody2D rb, pl;
    private BoxCollider2D bc;
    private BossHealth healthCS;
    private Animator animator;
    private List<GameObject> activeViruses;
    private string color;
    private float minSpeed = 2f, speed;
    private bool isBusy = false, isRecharged = true, facingLeft = true, isDisabled = false;
    private LayerMask ground;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pl = GameObject.Find("Main Character").GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        healthCS = GetComponent<BossHealth>();
        animator = GetComponent<Animator>();
        activeViruses = new();
        color = name.Split(" ")[0];
        ground = 1001 << GameObject.Find("Ground - Tile Map").layer;
        if (GameObject.Find("Platforms - Tile Map"))
            bc.excludeLayers = 1 << GameObject.Find("Platforms - Tile Map").layer;
        else bc.excludeLayers = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pl != null && Math.Abs(pl.position.x - rb.position.x) <= 15 && !animator.GetBool("isDead"))
        {
            float velx = 0;
            if (pl.position.x - rb.position.x < -.5f) velx = -1f;
            else if(pl.position.x - rb.position.x > .5f) velx = 1f;

            if ((velx < 0f && !facingLeft) || (velx > 0 && facingLeft)) Flip();

            if(!isBusy)
            {
                switch (color)
                {
                    case "Purple": PurpleBossAttack(velx); break;
                    case "Tricolor": TricolorBossAttack(velx); break;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D coll)       //This is to prevent the Boss from trapping you on the higher platforms.
    {
        List<string> Name = coll.gameObject.name.Split(" ").ToList();
        if (Name.Contains("Spray") && !CheckGroundStatus(ground - bc.excludeLayers))
            rb.AddForce(coll.relativeVelocity.normalized * 1000f);
    }

    private void PurpleBossAttack(float dir)
    {
        float healthRatio = healthCS.GetTotalHealth()/(healthCS.maxHealth*2f);
        speed = minSpeed * (2f - healthRatio);
        //speed = minSpeed * (-(float)Math.Log(healthRatio) + 1f);                          //If we want the Boss to get even faster, but more slowly.
        if (!isRecharged)                                                                   //Move towards the player if not ready to attack.
            rb.velocity = new Vector2(dir * speed, rb.velocity.y);
        else
        {
            if (pl.position.y - rb.position.y > .5f)                                          //If the player is too high...jump up to them.
                StartCoroutine(Jump(dir));
            else if (pl.position.y - rb.position.y < -1)
                StartCoroutine(Drop());
            else StartCoroutine(Dash(dir));                                                 //Otherwise, dash.
        }
    }
    private void TricolorBossAttack(float dir)
    {
        if (isRecharged)
        {
            speed = 1;
            StartCoroutine(JumpAndSlam(dir));
        }
    }

    IEnumerator Jump(float dir)
    {
        isBusy = true;
        LayerMask exclude = bc.excludeLayers;
        float jumpSpd = 5000;
        if (pl.position.y - rb.position.y > 8)
        {
            jumpSpd = 7000;
            if (exclude != 0) exclude = 0;
        }

        Vector2 dirV = new(dir, 0), offCenter = new(bc.bounds.center.x, bc.bounds.center.y - bc.bounds.extents.y * .5f);
        if (Physics2D.Raycast(offCenter, dirV, bc.bounds.extents.x * 1.1f, ground))
        {
            rb.velocity = -speed * dirV;
            yield return new WaitForSeconds(1f);
        }

        animator.SetBool("isJumping", true);
        yield return new WaitForSeconds(.25f);          //Waiting for transistion to start.
        bc.offset = new Vector2(bc.offset.x, -.13f);    //The jump anim is offset from the idle one.

        // Apply jump force
        rb.AddForce(new Vector2(dir * 1500f, jumpSpd));
        yield return new WaitUntil(() => rb.velocity.y < 0);
        if (CheckNoGroundOverlap()) bc.excludeLayers = exclude;
        animator.SetBool("isJumping", false);
        yield return new WaitForSeconds(.25f);          //Waiting for anim transition to end.
        bc.offset = new Vector2(bc.offset.x, 0);
        yield return new WaitUntil(() => CheckGroundStatus(ground - bc.excludeLayers));

        isBusy = false;
        StartCoroutine(Recharge());
    }
    IEnumerator Dash(float dir)
    {
        isBusy = true;
        Animator animator = GetComponent<Animator>();

        animator.SetBool("isDashing", true);
        //animator.enabled = false;
        yield return new WaitForSeconds(.5f);
        //animator.enabled = true;
        rb.AddForce(new Vector2(dir * 5000, 0f));
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("isDashing", false);
        isBusy = false;
        StartCoroutine(Recharge());
    }
    IEnumerator Drop()
    {
        isBusy = true;
        bc.excludeLayers = 1 << GameObject.Find("Platforms - Tile Map").layer;
        yield return new WaitUntil(() => Physics2D.OverlapAreaAll(bc.bounds.min, bc.bounds.max, bc.excludeLayers).Length == 0);
        isBusy = false;
        StartCoroutine(Recharge());
    }
    IEnumerator JumpAndSlam(float dir)
    {
        isBusy = true;
        rb.gravityScale = .1f;
        rb.AddForce(new Vector2(dir * 1500f, 2000));
        yield return new WaitUntil(() => rb.velocity.y < 0);
        rb.gravityScale = 10f;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.down * 5000);
        animator.SetBool("isSpawning", true);
        yield return new WaitUntil(() => CheckGroundStatus(ground - bc.excludeLayers));
        rb.gravityScale = 1f;
        yield return new WaitForSeconds(1f);
        animator.speed = 0;
        if (!isDisabled && activeViruses.Count < 5 && !animator.GetBool("isDead"))
        {
            System.Random random = new();
            StartCoroutine(Vibrate());
            while (!isDisabled && activeViruses.Count < 5 && !animator.GetBool("isDead"))
            {
                int rand = random.Next(0, viruses.Count);
                while (viruses[rand].name.Split(" ")[0] == "Yellow" && activeViruses.Count(x => x.name.Split(" ")[0] == "Yellow") > 0)
                    rand = random.Next(0, viruses.Count);
                GameObject virus = Instantiate(viruses[rand]);
                virus.transform.localPosition = transform.position - new Vector3(0, 1f);
                virus.GetComponent<Virus>().OnDeath += (virus) => { activeViruses.Remove(virus); };
                activeViruses.Add(virus);
                yield return new WaitForSeconds(1f);
            }
            for (int i = 0; i < 5 - activeViruses.Count && !animator.GetBool("isDead"); i++)//Added because Death waits
                yield return new WaitForSeconds(1f);
        }
        else for (int i = 0; i < 5 && !animator.GetBool("isDead"); i++)//so animation may not show before level continuation.
                yield return new WaitForSeconds(1f);
        animator.speed = 1;
        yield return new WaitForSeconds(1f);
        animator.SetBool("isSpawning", false);
        isBusy = false;
        StartCoroutine(Recharge());
    }
    IEnumerator Recharge()
    {
        isRecharged = false;
        yield return new WaitForSeconds(3f);
        isRecharged = true;
    }
    public void Disable()
    {
        isDisabled = true;
    }
    public void Enable()
    {
        isDisabled = false;
    }
    IEnumerator Vibrate()
    {
        Vector3 og = transform.position, vib = new(.25f, 0, 0);
        for(float i = 0; i <= activeViruses.Count+.2f; i += Time.deltaTime)
        {
            if (!animator.GetBool("isDead"))
            {
                transform.position += vib;
                vib = -vib;
            }
            Debug.Log("i="+i+"<=Count:"+activeViruses.Count+(i<=activeViruses.Count));
            yield return null;
        }
        transform.position = og;
    }
    private bool CheckGroundStatus(LayerMask g)
    {
        return Physics2D.OverlapAreaAll(bc.bounds.min, bc.bounds.max, g).Length > 0;
    }
    private bool CheckNoGroundOverlap()
    {
        return Physics2D.OverlapAreaAll(bc.bounds.min, bc.bounds.max, ground).Length == 0;
    }
    private void Flip()
    {
        // When called, toggle the check
        facingLeft = !facingLeft;
        // Tranform the player object 180 degree, facing them the opposite way
        //transform.Rotate(0, 180, 0);
        GetComponent<SpriteRenderer>().flipX = !facingLeft;
        bc.offset = new Vector2(-bc.offset.x, 0);
        GameObject.Find("Health Bar Background").GetComponent<RectTransform>().position += new Vector3(bc.offset.x * 10f, 0);
    }
}
