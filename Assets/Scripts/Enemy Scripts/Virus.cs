using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Virus : MonoBehaviour
{
    private readonly Dictionary<string, List<string>> mixedColors = new()
    {{"Purple", new(){"Red","Blue"}}, {"Orange", new(){"Yellow","Red"}}, {"Green", new(){"Yellow","Blue"}}};
    
    private Rigidbody2D rb, pl;
    private GameObject particle;
    private EnemyAudioManager audioManagerScript;

    private string color;
    private bool isBusy = false, isRecharged = true, facingLeft = true;
    private readonly float knockback = 2.0f, groundDist = .1f;
    private float speedSlowdown = 1f;
    
    private LayerMask ground;
    private Vector3 spawn;

    public event Action<GameObject> OnDeath;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        color = name.Split(" ")[0];
        pl = GameObject.Find("Main Character").GetComponent<Rigidbody2D>();
        ground = 1 << GameObject.Find("Ground - Tile Map").layer;
        spawn = transform.position;
        GameObject audioManager = GameObject.FindGameObjectWithTag("Enemy Audio");
        audioManagerScript = audioManager.GetComponent<EnemyAudioManager>();   
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(pl.position.x - rb.position.x) <= 25 && Math.Abs(pl.position.y - rb.position.y) <= 5)
        {
            if (color == "Yellow")
            {
                particle = transform.Find("Yellow Particle").gameObject;
                YellowMinionAttack();
            }
            else if (Math.Abs(pl.position.x - rb.position.x) <= 10 && Math.Abs(pl.position.y - rb.position.y) <= 5)
            {
                float distX = pl.position.x - rb.position.x, distY = pl.position.y - rb.position.y,
                      velx = distX < 0 ? -1f : 1f, vely = distY < 0 ? -1f : 1f;

                switch (color)
                {
                    case "Red": rb.velocity = new Vector2(velx * speedSlowdown, rb.velocity.y); break;
                    case "Blue": rb.velocity = new Vector2(velx *speedSlowdown, Math.Abs(distY) > 1.51f ? vely : -vely); break;
                    case "Purple": if (!isBusy) PurpleMinionAttack(velx *speedSlowdown); break;
                }
            }
        }
        CheckGroundStatus();
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Boss")) Physics2D.IgnoreCollision(coll.collider, GetComponent<Collider2D>());
        GameObject collidedWith = coll.gameObject;
        string[] tagA = collidedWith.tag.Split(" ");
        string col = tagA[0], proj = "";

        if (tagA.Length >= 3) proj = tagA[2];

        if (mixedColors.TryGetValue(color, out List<string> mix) && mix.Contains(col))
        {
            Animator anim = GetComponent<Animator>();
            if (!(anim.GetBool("isNotRed") || anim.GetBool("isNotBlue")))
                anim.SetBool("isNot" + col, true);
            else if ((anim.GetBool("isNotRed") && col != "Red") || (anim.GetBool("isNotBlue") && col != "Blue"))
            {
                DataCollector.tempScore += proj == "Cone" ? 400 : 200;
                HandleDeath();
            }
        }
        else if (col == color)
        {
            DataCollector.tempScore += proj == "Cone" ? 200 : 100;
            HandleDeath();
        }
        else if (proj == "Cone") ApplyKnockback(coll);
        if (proj == "Bullet") Destroy(collidedWith);
    }

    void ApplyKnockback(Collision2D coll)
    {
        Vector2 knockbackDirect = (transform.position - coll.transform.position).normalized;
        rb.AddForce(knockbackDirect * knockback, ForceMode2D.Impulse);
        //StartCoroutine(Stun());
    }

    public void CloudSlow(float factor){
        speedSlowdown = factor;
    }

    public void OutCloud(){
        speedSlowdown = 1f;
    }
    private bool CheckGroundStatus()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundDist, ground);
    }
    private void PurpleMinionAttack(float dir)
    {
        if ((dir < 0f && !facingLeft) || (dir > 0 && facingLeft)) Flip();
        StartCoroutine(Dash(dir));
    }
    private void YellowMinionAttack()
    {
        if (isRecharged)
        {
            Vector3 target = new(pl.position.x, pl.position.y + 3f);
            transform.position = Vector3.MoveTowards(transform.position, target, .5f);

            if (!isBusy && Vector3.Distance(transform.position, target) < 1f)
                StartCoroutine(DropParticle());
        }
        else transform.position = Vector3.MoveTowards(transform.position, spawn, .25f);
    }
    /*IEnumerator Jump(float dir)
    {
        isBusy = true;
        CircleCollider2D cc = gameObject.GetComponent<CircleCollider2D>();
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        transform.position *= new Vector2(1f, .95f);
        transform.localScale *= new Vector2(1.25f, .5f);
        cc.radius *= .5f;
        yield return new WaitForSeconds(.5f);

        transform.localScale *= new Vector2(.4f, 4f);
        cc.radius *= 2f;
        rb.AddForce(new Vector2(dir * 250, 350));
        yield return new WaitForSeconds(1f);

        transform.localScale *= new Vector2(2f, .5f);
        groundDist = cc.radius * 1.1f;
        yield return new WaitUntil(CheckGroundStatus());

        transform.position *= new Vector2(1f, .95f);
        transform.localScale *= new Vector2(1.25f, .5f);
        cc.radius *= .5f;
        yield return new WaitForSeconds(.5f);

        transform.localScale *= new Vector2(.8f, 2f);
        cc.radius *= 2f;
        groundDist = cc.radius * 1.1f;
        yield return new WaitForSeconds(3f);

        isBusy = false;
    }*/
    IEnumerator Dash(float dir)
    {
        isBusy = true;

        float height = GetComponent<BoxCollider2D>().size.y,    
              heightDiff = height * -.25f;
        transform.position += new Vector3(0f, heightDiff);      
        transform.localScale *= new Vector2(.5f, 1.25f);
        yield return new WaitForSeconds(.5f);

        heightDiff = height * .6f;
        transform.position += new Vector3(0f, -2f * heightDiff);
        transform.localScale *= new Vector2(4f, .4f);
        rb.AddForce(new Vector2(dir * 400, 0f));
        yield return new WaitForSeconds(1.5f);
     
        transform.localScale *= new Vector2(.5f, 2f);
        yield return new WaitForSeconds(2.5f);

        isBusy = false;
    }
    IEnumerator DropParticle()
    {
        isBusy = true;
        yield return new WaitForSeconds(3f);
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        yield return new WaitUntil(() => Physics2D.OverlapAreaAll(bc.bounds.min, bc.bounds.max, ground).Length == 0);
        particle.transform.localPosition = transform.position - new Vector3(0, .9f);
        Instantiate(particle).SetActive(true);
        yield return new WaitForSeconds(.5f);
        isRecharged = false;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, spawn) < 1f);
        StartCoroutine(Recharge());
        isBusy = false;
    }
    IEnumerator Recharge()
    {
        isRecharged = false;
        yield return new WaitForSeconds(3f);
        isRecharged = true;
    }
    /*IEnumerator Stun()            //Stun is in time-out. It's not playing well with others.
    {
        isBusy = true;
        bool recharchedIs = isRecharged;
        isRecharged = false;
        yield return new WaitForSeconds(2f);
        isBusy = false;
        isRecharged = rechargedIs;
    }*/
    private void Flip()
    {
        facingLeft = !facingLeft;
        GetComponent<SpriteRenderer>().flipX = !facingLeft;
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.offset = new Vector2(-bc.offset.x, 0);
    }

    public void HandleDeath()
    {
        OnDeath?.Invoke(gameObject);
        //audioSource.PlayOneShot(virusDeath, 0.7f);
        audioManagerScript.playEnemySFX(audioManagerScript.virusDeath);
        DataCollector.tempViruses++;
        DataCollector.tempPoints++;
        Destroy(gameObject);
    }
}
