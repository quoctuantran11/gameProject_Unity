using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    public int speed = 2;
    public int attackDamage = 10;
    public float attackRange = 2f; // also act as distance to a waypoint in order to change waypoint
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

    Path path;
    int currentWaypoint= 0 ;
    bool reachedTarget = false;
    Seeker seeker;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetLayerMask = LayerMask.GetMask("Hero");
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
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
            #region A* pathfinding

            if (path == null)
                return;
            if(currentWaypoint >= path.vectorPath.Count)
            {
                reachedTarget = true;
                if (Time.time >= attackCooldown)
                    animator.SetTrigger("Attack"); // Attack() is called in attack animation
                return;
            }
            else
            {
                reachedTarget = false;
            }

            MoveTowardPlayer();
            #endregion
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, player.position, OnPathComplete);
    }

    private void MoveTowardPlayer() 
    {
        Vector3 direction = ((Vector3)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector3 force = direction * speed * Time.deltaTime * 50;
        rb.AddForce(force);
        animator.SetFloat("Speed", speed);

        float distance = Vector3.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < attackRange)
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
            transform.localScale = new Vector3(-0.7f, 1, 1);
        else
            transform.localScale = new Vector3(0.7f, 1, 1);

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
        UIManager.instance.UpdateEnemyUI(maxHealth, currentHealth, thumbnailSprite);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsAlive", false);
        rb.AddRelativeForce(new Vector2(3, 5), ForceMode.Impulse); // ragdoll

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
