using UnityEngine;

public class Killable : MonoBehaviour
{
    [SerializeField] GameObject gore;
    [SerializeField] ParticleSystem killFX;
    [SerializeField] AudioClip deathSound;
    private Transform initialPosition;
    private L_Goal goal;
    private bool dead;

    public virtual void Die()
    {
        if (dead) return;
        dead = true;

        if (goal != null)
        goal.MonkeyKilled();

        gore.SetActive(true);
        killFX.Play();
        if (AudioManager.Instance)
        AudioManager.Instance.PlaySound(deathSound, 0.8f);
    }
    protected virtual void HandleReset()
    {
        dead = false;

        transform.position = initialPosition.position;
        transform.rotation = initialPosition.rotation;

        gore.SetActive(false);
    }

    public virtual void SetAsTarget(L_Goal manager)
    {
        goal = manager;
    }

    protected virtual void Start()
    {
        initialPosition = transform;
        GameManager.Instance.OnGameReset += HandleReset;
    }

    protected virtual void OnDestroy()
    {
        if (GameManager.Instance)
        GameManager.Instance.OnGameReset -= HandleReset;
    }
}