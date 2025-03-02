using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class player : MonoBehaviour
{
    private bool isWon = false;
    public float fallThreshold = -15f; // Adjust this based on your level design
    public GameObject gameVictoryUi;
    public GameObject gameOverUI;
    public int currentCoin = 0;
    public Text CurrentCoinText;
    public Text MaxHealthText;
    public Animator animator;
    private float movement;
    public float speed = 7f;
    public float jumpHeight = 10f;
    private bool facingRight = true;
    public Rigidbody2D rb;
    private bool isGround = true;
    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public LayerMask targetLayer;
    public int maxHealth = 15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindAnyObjectByType<sound>().backgroundmusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWon)
        {
            animator.SetFloat("Run", 0);
            movement = 0;
            speed = 0;
            return;
        }
        CurrentCoinText.text = currentCoin.ToString();
        if (maxHealth <= 0)
        {
            Die();
        }
        MaxHealthText.text = maxHealth.ToString();
        movement = Input.GetAxis("Horizontal"); // a=-1,d=1,w=0,s=0;
        if (movement < 0f && facingRight == true)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.W)) && isGround == true)
        {
            jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }
        if (Mathf.Abs(movement) > 0.1f) // Adjust the threshold as needed
        {
            animator.SetFloat("Run", 1f);
        }
        else
        {
            animator.SetFloat("Run", 0f);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F))
        {
            int randomIndex = Random.Range(0, 3);
            if (randomIndex == 0)
            {
                FindAnyObjectByType<sound>().Sword();
                animator.SetTrigger("Attack1");
            }
            else if (randomIndex == 1)
            {
                FindAnyObjectByType<sound>().Sword();
                animator.SetTrigger("Attack2");
            }
            else if (randomIndex == 2)
            {
                FindAnyObjectByType<sound>().Sword();
                animator.SetTrigger("Attack3");
            }
        }

        // Add rolling functionality
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(PerformRoll());
        }

        if (transform.position.y < fallThreshold) // Adjust the value based on your level
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * speed;
    }

    void jump()
    {
        // Jumping code
        Vector2 velocity = rb.velocity;
        velocity.y = jumpHeight;
        rb.velocity = velocity;
    }

    public void Attack()
    {
        Collider2D hitInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayer);
        if (hitInfo)
        {
            if (hitInfo.GetComponent<enemyknight>() != null)
            {
                hitInfo.GetComponent<enemyknight>().EnemyTakeDamage(1);
            }
            if(hitInfo.GetComponent<samurai>() != null)
            {
                hitInfo.GetComponent<samurai>().EnemyTakeDamage(1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
        if (collision.gameObject.tag == "KEY")
        {
            gameVictoryUi.SetActive(true);
            isWon = true;
            Destroy(collision.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            currentCoin++;
            collision.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collect");
            Destroy(collision.gameObject, 1f);
            FindAnyObjectByType<sound>().Coinsound();
        }
        if (collision.gameObject.tag == "Trap")
        {
            Die();
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damage;
    }

    void Die()
    {
        Debug.Log("Player died");
        Destroy(this.gameObject);
        gameOverUI.SetActive(true);
    }

    private IEnumerator PerformRoll()
    {
        animator.SetBool("Roll", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("Roll", false);
    }
}

