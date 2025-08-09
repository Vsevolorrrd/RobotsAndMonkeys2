using UnityEngine;

public class Collectable : MonoBehaviour
{
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

        // Collider2D col = GetComponent<Collider2D>();
    }

    public virtual void Drop(Transform currentItem)
    {
        transform.SetParent(null);

        transform.position = currentItem.position;
    }
}
