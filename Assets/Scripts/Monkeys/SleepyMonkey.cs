using UnityEngine;

public class SleepyMonkey : Killable
{
    [SerializeField] AudioClip snoring;
    [SerializeField] GameObject legs;
    [SerializeField] GameObject noLegs;

    public override void Die()
    {
        base.Die();
        noLegs.SetActive(true);
        legs.SetActive(false);
    }
    protected override void HandleReset()
    {
        base.HandleReset();
        noLegs.SetActive(false);
        legs.SetActive(true);
    }
    protected override void Start()
    {
        base.Start();
        Snoring();
    }
    private void Snoring()
    {
        if (snoring == null) return;
        AudioManager.Instance.PlaySound(snoring, 0.7f);
        float time = snoring.length;
        Invoke("Snoring", time);
    }
}