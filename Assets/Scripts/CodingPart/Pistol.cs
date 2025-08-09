using UnityEngine;

public class Pistol : Collectable
{
    [SerializeField] Sprite pistol;
    [SerializeField] Sprite pistolWithTape;
    private SpriteRenderer pistolRenderer;

    private void Awake()
    {
        pistolRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnCollect(Transform playerTransform, Collectable currentItem)
    {
        base.OnCollect(playerTransform, currentItem);
        pistolRenderer.sprite = pistolWithTape;
    }
    public override void Drop(Transform currentItem)
    {
        base.Drop(currentItem);
        pistolRenderer.sprite = pistol;
    }
}
