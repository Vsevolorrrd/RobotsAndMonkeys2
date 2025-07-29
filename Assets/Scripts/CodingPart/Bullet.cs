using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 4f;
    private Vector2 moveDirection;

    void Update()
    {
        transform.position = (Vector2)transform.position + moveDirection * speed * Time.deltaTime;
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Killable>())
        {
            Killable monkey = other.GetComponent<Killable>();
            monkey.AboutToDie();
            monkey.Die();
            Debug.Log("Triggered monkey");
        }
        if (!other.GetComponent<Minion>())
        {
            Destroy(gameObject);
        }
        Debug.Log("Triggered something else");
    }
}
