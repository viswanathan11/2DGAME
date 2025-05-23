using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class enemy2: MonoBehaviour
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
    public float attackRadius=2f;
     public LayerMask AttackLayer;
     public int maxHealth=3;

    

    // Update is called once per frame run every single time
    void Update()
    {if(maxHealth<=0){
        Die();
    }
    if(player==null){
        animator.SetBool("Playerdead",true);
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

        if (playerinRange)
        {
            if (transform.position.x < player.position.x && facingLeft)
            {
                // Enemy will face right when player is on the right side
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (transform.position.x > player.position.x && !facingLeft)
            {
                // Enemy will face left when player is on the left side
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }

            if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                // If player is out of range, enemy will chase the player
                animator.SetBool("Attack", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                // If player is in range, enemy will attack the player
                animator.SetBool("Attack", true);
            }
        }
        else
        {
            // Patrol logic
            animator.SetBool("Attack", false);
            transform.Translate(Vector2.left * walkSpeed * Time.deltaTime);
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
    
public void EnemyAttack(){
    Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position,attackRadius,AttackLayer);//it will return the collider of the object that is in the range of the attack point
    if(collInfo){
    if(collInfo.GetComponent<player>()!=null){
        collInfo.GetComponent<player>().PlayerTakeDamage(1);
    }
    }
 }
public void EnemyTakeDamage(int damage){
    if(maxHealth<=0){
        return;//whatever line after it will not be executed
    }
    maxHealth-=damage;

}
public void Die(){
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
        if(attackPoint!=null){
            Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);
        }
    }
}
