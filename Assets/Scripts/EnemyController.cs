using System.Collections;
using UnityEngine;
using Pathfinding;

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
    public GameObject hitEffectPrefab;

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

    Path path;
    int currentWaypoint = 0;
    bool reachedTarget = false;
    Seeker seeker;

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

        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void OnPathComplete(Path p) // reset waypoint
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, player.position, OnPathComplete);
    }

    void Update()
    {
        FacePlayer();
        walkTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(currentHealth > 0)
        {
            #region New pathfinding

            if (path == null)
                return;
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedTarget = true;
                if (Time.time >= attackCooldown && FindObjectOfType<Player>().health > 0)
                {
                    animator.SetTrigger("Attack"); // Attack() is called in attack animation
                    attackCooldown = Time.time + (float)(6.0f / (attackRate * 1.0f)) - 0.5f;
                }
                return;
            }
            else
            {
                reachedTarget = false;
            }

            MoveTowardPlayer();

            #endregion


            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1),
                rb.position.y,
                Mathf.Clamp(rb.position.z, minHeight, maxHeight));
        }
    }

    private void MoveTowardPlayer()
    {
        Vector3 direction = ((Vector3)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector3 force = direction * speed * Time.deltaTime * 50;
        rb.AddForce(force);
        animator.SetFloat("Speed", speed);

        float distance = Vector3.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < attackRange)
        {
            currentWaypoint++;
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
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1,1, 1);
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

        ShowHitEffect();
        animator.SetTrigger("Get Hurt");

        PlaySong(collisionSound);

        UIManager.instance.UpdateEnemyUI(maxHealth, currentHealth, thumbnailSprite);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ShowHitEffect()
    {
        GameObject sparkObj = Instantiate(hitEffectPrefab);
        sparkObj.transform.position = transform.position + new Vector3(0.3f,1.2f,0);
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
                this.setStats(EnemyStatsManager.Instance.HighDamageEnemyStatsAtLevel(5));
                sprite.color = new Color(0.749f, 0.1215f, 0.1215f);
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.749f, 0.1215f, 0.1215f);
                transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.green; 
                break;
            case 3:
                this.setStats(EnemyStatsManager.Instance.HighHealthEnemyStatsAtLevel(5));
                sprite.color = new Color(0, 0.588f, 0.862f);
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0, 0.588f, 0.862f);
                transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.magenta;
                break;
            case 4:
                this.setStats(EnemyStatsManager.Instance.HighMobilityEnemyStatsAtLevel(5));
                sprite.color = new Color(0.98f, 0.84f, 0.1176f);
                transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0.98f, 0.84f, 0.1176f);
                transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            default:
                this.setStats(EnemyStatsManager.Instance.enemyStatsAtLevel(5));
                break;
        }
    }
}
