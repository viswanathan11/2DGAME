using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{
    public  Animator animator;
    private float movement;
    public float speed = 7f;
    public float jumpHeight = 10f;
    private bool facingRight = true;
    public Rigidbody2D rb; 
    private bool isGround = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() 
    {
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
        if (Input.GetKeyDown(KeyCode.Space)&& isGround== true)
        {
            jump();
            isGround=false;
            animator.SetBool("Jump",true);
        }
        if(Mathf.Abs(movement)>.1f){//abs make -1 and 1 to 1
            animator.SetFloat("Run",1f);

        }
        else{
            animator.SetFloat("Run",0f);
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
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag=="Ground"){
            isGround=true;
            animator.SetBool("Jump",false);
        }
    }

}