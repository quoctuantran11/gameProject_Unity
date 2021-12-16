using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float speed = 2f;
    public float attackDamage = 10f;
    public float attackRange = 1f;
    public float attackRate = 1f; // number of attacks per second
    public Vector2 attackOffset; // origin of attack relative to 
    public LayerMask targetLayerMask; // Layer of player

    Transform player;
    Animator animator;
    Rigidbody rb;
    Transform groundCheck;
    bool onGround;
        
    float currentHealth;
    bool isFlipped = false;
    float zforce;
    float walkTimer;
    float attackCooldown = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        FacePlayer();
        walkTimer += Time.deltaTime;

        if (Vector3.Distance(player.position, rb.position)<= attackRange && Time.time >= attackCooldown)
        {
            animator.SetTrigger("Attack"); // Attack() is called in attack animation
        }
    }

    private void FixedUpdate()
    {
        if(currentHealth > 0)
        {
            Vector3 targetDistance = player.position - transform.position; // locate player
            float hForce = targetDistance.x / Mathf.Abs(targetDistance.x);

            if (walkTimer >= Random.Range(1f, 2f))
            {
                zforce = Random.Range(-1, 2);
                walkTimer = 0;
            }
            if (Mathf.Abs(targetDistance.x) < 1.5f)
            {
                hForce = 0;
            }
            rb.velocity = new Vector3(hForce * speed, 0, zforce * speed); // move toward player
            animator.SetFloat("Speed", Mathf.Abs(speed));
        }
    }

    public void FacePlayer()
    {
        if (player != null)
        {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;

            if (transform.position.x > player.position.x && isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
            else if (transform.position.x < player.position.x && !isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }
        }        
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, targetLayerMask); // player collide with attack
        if (colInfo != null)
        {
            // TODO: Player take damage
        }

        attackCooldown += Time.time + 1 / attackRate;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Get Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsAlive", false);
        rb.AddRelativeForce(new Vector2(3, 5), ForceMode.Impulse);

        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(Blink());
        this.enabled = false;
    }

    IEnumerator Blink()
    {
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();

        Color defaultColor = playerSprite.color;

        playerSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.05f);

        playerSprite.color = defaultColor;
    }
}
