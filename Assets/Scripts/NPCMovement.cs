using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints;   // Lista waypointów
    public float speed = 2f;        // Prêdkoœæ ruchu NPC

    private int currentWaypoint = 0;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (waypoints.Length == 0)
            Debug.LogWarning("Dodaj waypointy do NPC!");
    }

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        Vector2 targetPos = waypoints[currentWaypoint].position;
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);

        // Poruszaj Rigidbody, ¿eby dzia³a³y kolizje
        rb.MovePosition(newPos);

        // SprawdŸ czy osi¹gnêliœmy waypoint
        if (Vector2.Distance(rb.position, targetPos) < 0.1f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
                currentWaypoint = 0; // powtarzaj pêtlê waypointów
        }
    }
}
