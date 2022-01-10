using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float maxSpeed = 4;
    public float jumpForce = 400;
    public float minHeight, maxHeight;
    public int maxHealth = 10;
    public string playerName;
    public Sprite playerImage;
    public int attackDamage = 10;
    public float attackRange = 4f;
    public float attackRate = 1f;
    public AudioClip collisionSound, pushSound, jumpSound, healthItem;
    public GameObject hitEffectPrefab;

    public LayerMask targetLayerMask; // Layer of player
    float attackCooldown = 0f;

    int statType;

    private int currentHealth;
    private float currentSpeed;
    private float normalSpeed;
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    private bool onGround;
    private bool isKnock;
    private bool isDead = false;
    private bool facingRight = true;
    private bool jump = false;
    private bool isRun = false;
    private AudioSource audioS;
    public int health
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
        }
    }
    private float defense = 0;
    private float lifeSteal = 0;
    // Start is called before the first frame update
    void Start()
    {
        targetLayerMask = LayerMask.GetMask("Enemy");
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        this.initPlayerStats();
        currentHealth = maxHealth;
        audioS = GetComponent<AudioSource>();
        statType = MyGameManager.Instance.getType;
    }

    // Update is called once per frame
    void Update()
    {

        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        anim.SetBool("OnGround", onGround);
        anim.SetBool("Dead", isDead);

        if (Input.GetButtonDown("Jump") && onGround)
        {
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time > this.attackCooldown){
                anim.SetTrigger("Attack");
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isRun == false)
            {
                isRun = true;
            }
            else
            {
                isRun = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (!onGround)
            {
                z = 0;
            }

            rb.velocity = new Vector3(h * currentSpeed, rb.velocity.y, z * currentSpeed);

            if (onGround)
            {
                anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
            }

            if (h > 0 && !facingRight)
            {
                Flip();
            }
            else if (h < 0 && facingRight)
            {
                Flip();
            }

            if (jump)
            {
                jump = false;
                rb.AddForce(Vector3.up * jumpForce);
                PlaySong(jumpSound);
            }

            if (isRun)
            {
                currentSpeed = maxSpeed;
            }
            else
            {
                currentSpeed = normalSpeed;
            }

            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1),
                rb.position.y,
                Mathf.Clamp(rb.position.z, minHeight, maxHeight));
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }

    public void TooKDamage(int damage)
    {
        if (!isDead)
        {
            int damageReduce = (int)(damage * this.defense);
            damage -= damageReduce;
            currentHealth -= damage;
            ShowHitEffect();
            anim.SetTrigger("Hurt");
            FindObjectOfType<UIManager>().UpdateHealth(currentHealth * 100 / maxHealth);
            PlaySong(collisionSound);

            if (currentHealth <= 0)
            {
                isDead = true;
                anim.SetTrigger("Dead");
                FindObjectOfType<GameManager>().lives--;
                if (facingRight)
                {
                    rb.AddForce(new Vector3(-3, 5, 0), ForceMode.Impulse);
                }
                else
                {
                    rb.AddForce(new Vector3(3, 5, 0), ForceMode.Impulse);
                }
            }
        }
    }

    void ShowHitEffect()
    {
        GameObject sparkObj = Instantiate(hitEffectPrefab);
        sparkObj.transform.position = transform.position + new Vector3(0.3f, 1.2f, 0);
    }

    public void GetKnock(int damage)
    {
        if (!isDead)
        {
            if (damage >= 5)
            {
                isKnock = true;
                currentHealth -= damage;
                anim.SetTrigger("Knockout");
                FindObjectOfType<UIManager>().UpdateHealth(currentHealth * 100 / maxHealth);
            }
        }
    }

    public void Getup()
    {
        if (isKnock)
        {
            anim.SetTrigger("Hurt");
            isKnock = false;
        }
    }

    public void Attack()
    {
        Collider[] colInfo = Physics.OverlapSphere(transform.position, attackRange, targetLayerMask); // player collide with attack
        if (colInfo != null)
        {
            foreach (Collider enemy in colInfo)
            {
                enemy.GetComponent<EnemyController>().TakeDamage(attackDamage);
                int healthRecover = (int)(attackDamage * this.lifeSteal);
                this.currentHealth += healthRecover;
                this.currentHealth = this.currentHealth > this.maxHealth ? this.maxHealth : this.currentHealth;
                FindObjectOfType<UIManager>().UpdateHealth(currentHealth * 100 / maxHealth);
            }
        }

        attackCooldown = Time.time + (float)(1 / attackRate);
    }

    public void PlaySong(AudioClip clip)
    {
        audioS.clip = clip;
        audioS.Play();
    }

    void PushSound()
    {
        PlaySong(pushSound);
    }
    
    void PlayerRespawn()
    {
        if (FindObjectOfType<GameManager>().lives > 0)
        {
            
            FindObjectOfType<UIManager>().UpdateLives();
            currentHealth = maxHealth;
            FindObjectOfType<UIManager>().UpdateHealth(currentHealth * 100 / maxHealth);
            anim.Rebind();
            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            transform.position = new Vector3(minWidth, 10, -4);
            isDead = false;
        }
        else
        {
            FindObjectOfType<UIManager>().UpdateDisplayMessage("Game Over");
            Destroy(FindObjectOfType<GameManager>().gameObject);
            Invoke("LoadScene", 2f);
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene("Menu");
    }

    private void initPlayerStats(){
        PlayerStatsManager statsManager = new PlayerStatsManager(statType);
        PlayerStats stats = statsManager.playerStatsAtLevel(MyGameManager.Instance.getLevel);
        this.maxHealth = stats.maxHealth;
        this.normalSpeed = stats.speed;
        this.maxSpeed = stats.maxSpeed;
        this.attackDamage = stats.attackDamage;
        this.attackRate = stats.attackRate;
        this.lifeSteal = stats.lifeSteal;
        this.defense = stats.defense;
    }
}
