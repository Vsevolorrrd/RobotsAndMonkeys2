using UnityEngine;

public class Cat : Collectable
{
    public override void OnCollect(Transform playerTransform, Collectable currentItem)
    {
        base.OnCollect(playerTransform, currentItem);

        // AudioManager.Instance.PlaySound(); here should be sound of happy meow
    }

    public override void Drop(Transform playerTransform)
    {
        // AudioManager.Instance.PlaySound(); here should be sound of sad meow
        base.Drop(playerTransform);
    }

    void Update()
    {
        // AudioManager.Instance.PlaySound(); here should loop sound of purring or Imperor Meow from StarWars youtube video
    }
}
