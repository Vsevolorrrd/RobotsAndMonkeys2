using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 3f;
    private Vector2 moveDirection;

    void Update()
    {
        transform.position = (Vector2)transform.position + moveDirection * speed * Time.deltaTime;
    }

    public void SetRotation(Transform playerTransform)
    {
        transform.rotation = playerTransform.rotation * Quaternion.Euler(0, 0, 90f);
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
            Destroy(gameObject);
        }

        if (!other.GetComponent<Minion>() || !other.GetComponent<Killable>())
        {
            StartCoroutine(DestroyBullet());
        }
        Debug.Log("Triggered something else");
    }

    public IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
