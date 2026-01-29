using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Dialogue")]
    public GameObject chmurka;
    
    [Header("Movement")]
    public float speed = 2f;
    public float patrolDistance = 3f;

    private Vector2 startPos;
    private int direction = 1;
    
    Animator animator;
    bool isStuned = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;

        if (chmurka != null)
            chmurka.SetActive(false);
    }

    void Update()
    {
        if (isStuned) return;

        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (Vector2.Distance(startPos, transform.position) >= patrolDistance)
        {
            direction *= -1;
            startPos = transform.position;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && chmurka != null)
        {
            chmurka.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && chmurka != null)
        {
            chmurka.SetActive(false);
        }
    }

    public void Stun(float duration)
    {
        isStuned = true;

        if (animator != null)
            animator.speed = 0f;

        StartCoroutine(StunTimer(duration));
    }

    private IEnumerator StunTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        isStuned = false;

        if (animator != null)
            animator.speed = 1f;
    }
}