using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip parrySound;
    private float speed;
    public float currentSpeed;
    private float speedMultiplier;
    private float horizontal;
    private ParryableController lastParriedEnemy;

    private const float normalSpeed = 5.0f;
    private const float crawlingSpeed = 2.5f;
    private const float normalSpeedMultiplier = 1.0f;
    private const float parrySpeedMultiplier = 1.3f;
    private const float parryWindowTime = 0.5f;
    private const float parryCooldown = 7.5f;
    private const float stunTime = 5.0f;
    private Vector2 idleSize = new(0.16f, 0.54f);
    private Vector2 crawlingSize = new(0.16f, 0.2f);

    private bool isFacingRight = true;
    private bool isCrawling = false;
    private bool isMoving = false;


    public bool isParryWindowOpen = false;
    public bool isParryOnTimeout = false;

    public InputAction leftAction;
    public InputAction rightAction;
    public InputAction upAction;
    public InputAction downAction;
    public InputAction interactAction;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform cellingCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private BoxCollider2D boxCollider2D;
    

    void Start() {
        animator = GetComponent<Animator>();

        Vector2 position = transform.position;
        position.x = 6;
        position.y = 11;
        transform.position = position;

        speed = normalSpeed;
        speedMultiplier = normalSpeedMultiplier;

        leftAction.Enable();
        rightAction.Enable();
        downAction.Enable();
        upAction.Enable();
        interactAction.Enable();
    }

    void Update() {

        Parry(); 
        Move();
    }

    void Move() {
        isMoving = false;
        horizontal = 0.0f;
        
        if (leftAction.IsPressed()) {
            isMoving = true;
            horizontal = -1.0f;
        }

        if (rightAction.IsPressed()){
            isMoving = true;
            horizontal = 1.0f;
        }

        animator.SetBool("isMoving", isMoving);

        Crawl();
        Flip();
        
        currentSpeed = speed * speedMultiplier;
        rb.linearVelocity = new Vector2(horizontal * currentSpeed, rb.linearVelocity.y);
    }

    void Crawl() {
        if (downAction.IsPressed() && !isMoving) {
            isCrawling = true; 
            boxCollider2D.size = crawlingSize;
            boxCollider2D.offset = new(0.0f, 0.1f);
            speed = crawlingSpeed;
        }

        if (upAction.IsPressed() && !isMoving && !IsCellingAbove()) {
            isCrawling = false;
            boxCollider2D.size = idleSize;
            boxCollider2D.offset = new(0.0f, 0.267f);
            speed = normalSpeed;
        }

        animator.SetBool("isCrawling", isCrawling);
    }

    void Flip() {
        if (isFacingRight && horizontal < 0.0f || !isFacingRight && horizontal > 0.0f)         {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1.0f;
            transform.localScale = localScale;
        }
    }

    bool IsCellingAbove() {
        return Physics2D.OverlapCircle(cellingCheck.position, 0.2f, groundLayer);
    }

    bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        lastParriedEnemy = collider.GetComponent<ParryableController>();
        StartCoroutine(ParryWindow(parryWindowTime)); 
    }

    void Parry() {
        animator.SetBool("succesfullParry", false);

        if (interactAction.WasPressedThisFrame() && isParryWindowOpen && !isParryOnTimeout && !isCrawling) {        
            animator.SetBool("succesfullParry", true);
            isParryOnTimeout = true;
            speedMultiplier = parrySpeedMultiplier;
            StartCoroutine(ParryCooldown(parryCooldown));
            if (lastParriedEnemy) {
                lastParriedEnemy.Stun(stunTime);
            }
        } 
    }

    IEnumerator ParryCooldown(float time) {
        yield return new WaitForSecondsRealtime(time);
        isParryOnTimeout = false;
        speedMultiplier = normalSpeedMultiplier;
    }

    IEnumerator ParryWindow(float time) {
        isParryWindowOpen = true;
        yield return new WaitForSeconds(time);
        isParryWindowOpen = false;
    }

    IEnumerator ParryFreeze(float time) {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1.0f;
    }

    public void PlayParrySound() {
        audioSource.PlayOneShot(parrySound);
    }
}
