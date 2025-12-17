using UnityEngine;

public class TestMove : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(h, v).normalized;
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }
}
