using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public Vector2[] patrolPoints;
    public float speed = 2f;
    public float reachDistance = 0.1f;

    private Rigidbody2D rb;
    private int currentIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 target = patrolPoints[currentIndex];
        Vector2 direction = (target - rb.position).normalized;

        rb.velocity = direction * speed;

        if (Vector2.Distance(rb.position, target) < reachDistance)
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
        }
    }
}