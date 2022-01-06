using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum Type{
        normal = 1, highDamage = 2, Tanking = 3, fast = 4, normalV1 = 5, normalV2 = 6
    }
    public int maxHealth = 100;
    public float minHeight, maxHeight;
    public int speed = 2;
    public int attackDamage = 10;
    public float attackRange = 2f;
    public int attackRate = 1; // number of attacks per second
    public LayerMask targetLayerMask; // Layer of player
    public Sprite thumbnailSprite;
    public AudioClip collisionSound, knockSound;

    Transform player;
    protected Animator animator;
    Rigidbody rb;
    Transform groundCheck;
    bool onGround;
        
    int currentHealth;
    protected bool isFlipped = false;
    float zforce;
    float walkTimer;
    float attackCooldown;
    SpriteRenderer sprite;
    private AudioSource audioS;
    public int health
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
        }
    }

    void Start()
    {
        this.attackCooldown = 0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetLayerMask = LayerMask.GetMask("Hero");
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        audioS = GetComponent<AudioSource>();
        randomEnemyType();
    }

    void Update()
    {
        FacePlayer();
        walkTimer += Time.deltaTime;

        if (Vector3.Distance(player.position, rb.position) <= attackRange && Time.time >= attackCooldown)
        {
            if (FindObjectOfType<Player>().health > 0){
                rb.velocity = Vector3.zero;
                animator.SetTrigger("Attack"); // Attack() is called in attack animation
                attackCooldown = Time.time + (float)(6.0f / (attackRate * 1.0f)) - 0.5f;
            }
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

            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0,0,10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1),
                rb.position.y,
                Mathf.Clamp(rb.position.z, minHeight, maxHeight));
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
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Get Hurt");
        PlaySong(collisionSound);

        UIManager.instance.UpdateEnemyUI(maxHealth, currentHealth, thumbnailSprite);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsAlive", false);
        animator.SetTrigger("Knock Down");
        rb.AddRelativeForce(new Vector2(3, 5), ForceMode.Impulse); // ragdoll

        PlaySong(knockSound);
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
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

    public void PlaySong(AudioClip clip)
    {
        audioS.clip = clip;
        audioS.Play();
    }

    protected void setStats(EnemyStats enemyStats){
        this.maxHealth = enemyStats.maxHealth;
        this.speed = enemyStats.speed;
        this.attackRate = enemyStats.attackRate;
        this.attackDamage = enemyStats.attackDamage;
    }

    public virtual void randomEnemyType(){
        int type = Random.Range(1, 6);
        switch(type){
            case 2:
                this.setStats(EnemyStatsManager.Instance.HighDamageEnemyStatsAtLevel(FindObjectOfType<GameManager>().defaultLevel));
                break;
            case 3:
                this.setStats(EnemyStatsManager.Instance.HighHealthEnemyStatsAtLevel(FindObjectOfType<GameManager>().defaultLevel));
                break;
            case 4:
                this.setStats(EnemyStatsManager.Instance.HighMobilityEnemyStatsAtLevel(FindObjectOfType<GameManager>().defaultLevel));
                break;
            default:
                this.setStats(EnemyStatsManager.Instance.enemyStatsAtLevel(FindObjectOfType<GameManager>().defaultLevel));
                break;
        }
    }
}
