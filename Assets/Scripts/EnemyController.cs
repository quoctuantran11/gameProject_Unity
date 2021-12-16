using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    public int speed = 2;
    public int attackDamage = 10;
    public float attackRange = 2f;
    public int attackRate = 1; // number of attacks per second
    public LayerMask targetLayerMask; // Layer of player
    public Sprite thumbnailSprite;

    Transform player;
    Animator animator;
    Rigidbody rb;
    Transform groundCheck;
    bool onGround;
        
    int currentHealth;
    bool isFlipped = false;
    float zforce;
    float walkTimer;
    float attackCooldown = 0f;
    SpriteRenderer sprite;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetLayerMask = LayerMask.GetMask("Hero");
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FacePlayer();
        walkTimer += Time.deltaTime;

        if (Vector3.Distance(player.position, rb.position) <= attackRange && Time.time >= attackCooldown)
        {
            rb.velocity = Vector3.zero;
            animator.SetTrigger("Attack"); // Attack() is called in attack animation
        }
    }

    private void FixedUpdate()
    {
        if(currentHealth > 0)
        {
            // Not a very good pathfinding script, can improve using A*

            Vector3 targetDistance = player.position - transform.position; // locate player
            if (targetDistance.magnitude > attackRange)
            {
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
            else animator.SetFloat("Speed", 0f);
        }
    }

    public void FacePlayer()
    {
        if (player != null)
        {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;

            if (transform.position.x > player.position.x && !isFlipped)
            {
                isFlipped = true;
            }
            else if (transform.position.x < player.position.x && isFlipped)
            {
                isFlipped = false;
            }
        }
        if (isFlipped)
            sprite.flipX = true;
        else
            sprite.flipX = false;
    }

    public void Attack()
    {
        Collider[] colInfo = Physics.OverlapSphere(transform.position, attackRange, targetLayerMask); // player collide with attack
        if (colInfo != null)
        {
            foreach(Collider hero in colInfo)
            {
                hero.GetComponent<Player>().TooKDamage(attackDamage);
            }
        }

        attackCooldown = Time.time + 1 / attackRate;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Get Hurt");
        FindObjectOfType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, thumbnailSprite);

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

    // corpses blink before disappear
    IEnumerator Blink()
    {
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();

        Color defaultColor = playerSprite.color;

        playerSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.5f);

        playerSprite.color = defaultColor;
    }
}
