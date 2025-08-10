using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Vector2 initialPosition;
    // private bool collected = false;



    public virtual void OnCollect(Transform playerTransform, Collectable currentItem)
    {
        if (currentItem != null)
        {
            currentItem.Drop(transform);
        }
        Debug.Log("Item collected. Setting new parrent.");
        transform.SetParent(playerTransform);

        transform.localPosition = Vector2.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        // Collider2D col = GetComponent<Collider2D>();
    }

    public virtual void Drop(Transform playerTransform)
    {
        transform.SetParent(null);

        transform.position = playerTransform.position;
    }

    void Start()
    {
        initialPosition = transform.position;
        GameManager.Instance.OnGameReset += HandleReset;
    }

    public virtual void HandleReset()
    {
        transform.SetParent(null);
        transform.position = initialPosition;
    }

    public void OnDestroy()
    {
        if (GameManager.Instance)
        GameManager.Instance.OnGameReset -= HandleReset;
    }
}
