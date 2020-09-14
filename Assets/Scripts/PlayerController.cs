using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sp;
    private AudioSource audioSource;

    private bool facingRight = true;
    public float xSpeed = 5f;
    public float jumpForce = 20f;
    public float superJumpForce = 25f;

    private bool grounded;
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    public Transform shootPosition;
    public GameObject shotPrefab;
    private bool shooting = false;
    public float shotTime = .5f;
    private IEnumerator shootingCoroutine;

    private bool superJump = false;
    [HideInInspector] public bool superJumpUnlocked = false;
    public LayerMask superGroundLayer;
    public ParticleSystem superParticles;

    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip superJumpSound;
    public AudioClip superJumpGroundSound;
    public AudioClip hurtSound;

    public static PlayerController instance;

    void Start() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        shootingCoroutine = ShootingLoop();
        superParticles.Stop();
    }

    void Update() {
        // Velocity
        float yVel = rb.velocity.y;
        float xVel = Input.GetAxis("Horizontal") * xSpeed;

        // Start Shooting
        if (!shooting && Input.GetAxisRaw("Fire1") > 0f) {
            shooting = true;
            StartCoroutine(shootingCoroutine);
        }
        
        // Stop shooting
        if (shooting && Input.GetAxisRaw("Fire1") <= 0f) {
            shooting = false;
            StopCoroutine(shootingCoroutine);
        }

        // Direction Facing Test
        if (xVel != 0f) {
            facingRight = xVel > 0f;
            transform.localScale = new Vector2(facingRight ? 1f : -1f, 1f);
        }

        // Super Jump Ground Check
        if (superJumpUnlocked) {
            bool wasSuper = superJump;
            superJump  = Physics2D.OverlapCircle(groundCheck.position, .02f, superGroundLayer);
            if (!wasSuper && superJump) {
                superParticles.Play();
                audioSource.PlayOneShot(superJumpGroundSound);
            } else if (wasSuper && !superJump) {
                superParticles.Stop();
                superParticles.Clear();
            }
        }

        // Grounded Check
        bool wasGrounded = grounded;
        grounded = Physics2D.OverlapCircle(groundCheck.position, .02f, groundLayer);
        if (!wasGrounded && grounded) {
            audioSource.PlayOneShot(landSound);
        }

        // Jump
        if (grounded && Input.GetAxis("Vertical") > 0) {
            grounded = false;
            if (superJump) {
                yVel = superJumpForce;
                audioSource.PlayOneShot(superJumpSound);
            } else {
                yVel = jumpForce;
                audioSource.PlayOneShot(jumpSound);
            }
            animator.SetTrigger("Jump");
        }

        // Set Rigidbody2D Velocity
        rb.velocity = new Vector2(xVel, yVel);

        // Set Animator Values
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Running", xVel != 0f);
        animator.SetBool("Shooting", shooting);
        animator.SetBool("Crouching", Input.GetAxisRaw("Vertical") < 0f);
    }

    // Coroutine that spawns shot prefabs
    IEnumerator ShootingLoop() {
        while (true) {
            GameObject shot = Instantiate(shotPrefab, shootPosition.position, Quaternion.identity);
            shot.GetComponent<ShotController>().right = facingRight;
            yield return new WaitForSeconds(shotTime);
        }
    }

    // Trigger hurt animation when touching enemy
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            animator.SetTrigger("Hurt");
            audioSource.PlayOneShot(hurtSound);
            rb.velocity = new Vector2(0, 8);
        }
    }
}
