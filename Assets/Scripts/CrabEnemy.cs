using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrabEnemy : MonoBehaviour {
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool moving;
    private bool facingRight = false;
    public float movingSpeed = 3f;

    private float timer = 0f;

    public int health = 5;

    public GameObject deathEffect;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        timer += Time.deltaTime;

        if (moving) {
            rb.velocity = new Vector2(facingRight ? movingSpeed : -movingSpeed, rb.velocity.y);
            if (timer > 2.5f) {
                timer = 0;
                moving = false;
            }
        } else {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            if (timer > 1.5f) {
                timer = 0;
                moving = true;
                facingRight = !facingRight;
                spriteRenderer.flipX = facingRight;
            }
        }
        animator.SetBool("walking", moving);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Shot") {
            health--;
            if (health == 0) {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

}
