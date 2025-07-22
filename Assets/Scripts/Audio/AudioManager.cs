using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource soundFXPrefab;

    [SerializeField] int maxSoundsPlaying = 25;
    private int currentSoundsPlaying = 0;

    [Header("UI Sound")]
    public AudioClip buttonSound;
    public AudioClip hoverSound;

    public void PlaySound(AudioClip audioClip, float volume = 1)
    {
        if (currentSoundsPlaying >= maxSoundsPlaying)
        return;  // Do not play the sound if the limit is exceeded

        AudioSource audioSource = Instantiate(soundFXPrefab, transform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
    public void PlayRandomSound(AudioClip[] audioClip, float volume = 1)
    {
        if (currentSoundsPlaying >= maxSoundsPlaying)
        return;  // Do not play the sound if the limit is exceeded

        int R = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(soundFXPrefab, transform.position, Quaternion.identity);

        audioSource.clip = audioClip[R];
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
