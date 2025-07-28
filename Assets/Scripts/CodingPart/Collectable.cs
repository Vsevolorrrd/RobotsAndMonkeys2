using UnityEngine;

public class Collectable : MonoBehaviour
{
    private bool collected = false;

    public void OnCollect(Transform playerTransform)
    {
        Debug.Log("Item collected. Destroying.");
        transform.SetParent(playerTransform);

        transform.localPosition = Vector2.zero;

        Collider2D col = GetComponent<Collider2D>();
    }
}
