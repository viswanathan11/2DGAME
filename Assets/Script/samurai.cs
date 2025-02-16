using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class samurai : MonoBehaviour
{
    public float walkSpeed = 2f;
    public LayerMask detectlayer;
    private bool facingLeft = true;
    public Transform player;
    private bool playerinRange = false;

    public float chaseSpeed = 4f;
    public Transform detectPoint;
    public float distance = 2.5f;
    public float attackRange = 2.5f;
    public Animator animator;
    public Transform attackPoint;
    public float attackRadius = 2f;
    public LayerMask AttackLayer;
    public int maxHealth = 3;
    private bool toggleAttack = false;
    private bool isAttacking = false;

    void Update()
    {
        if (maxHealth <= 0)
        {
            Die();
        }
        if (player == null)
        {
            animator.SetBool("Playerdead", true);
            return;
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            playerinRange = true;
        }
        else
        {
            playerinRange = false;
        }

        if (playerinRange && !isAttacking)
        {
            if (transform.position.x < player.position.x)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                facingLeft = true;
            }

            if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                animator.SetBool("Attack", false);
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                Debug.Log("Attack");
                toggleAttack = !toggleAttack;
                animator.SetBool("Attack", toggleAttack);
                animator.SetBool("Attack1", !toggleAttack);
                isAttacking = true;
                Invoke("ResetAttack", GetCurrentAnimationLength());
            }
        }
        else if (!playerinRange)
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Attack1", false);
            transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);
            RaycastHit2D hit = Physics2D.Raycast(detectPoint.position, Vector2.down, distance, detectlayer);

            if (hit.collider == null)
            {
                if (facingLeft)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    facingLeft = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    facingLeft = true;
                }
            }
        }
    }

    private float GetCurrentAnimationLength()
    {
        AnimatorStateInfo stateInfo;
        if (toggleAttack)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.length;
        }
        else
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.length;
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void EnemyAttack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, AttackLayer);
        if (collInfo)
        {
            if (collInfo.GetComponent<player>() != null)
            {
                collInfo.GetComponent<player>().PlayerTakeDamage(1);
            }
        }
    }

    public void EnemyTakeDamage(int damage)
    {
        Debug.Log("EnemyTakeDamage called with damage: " + damage);
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy died");
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (detectPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(detectPoint.position, Vector2.down * distance);
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
