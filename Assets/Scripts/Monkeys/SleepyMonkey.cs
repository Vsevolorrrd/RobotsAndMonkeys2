using UnityEngine;

public class SleepyMonkey : Killable
{
    [SerializeField] AudioClip snoring;
    protected override void Start()
    {
        base.Start();
        Snoring();
    }
    private void Snoring()
    {
        if (snoring == null) return;

        AudioManager.Instance.PlaySound(snoring);
        float time = snoring.length;
        Invoke("Snoring", time);
    }
}