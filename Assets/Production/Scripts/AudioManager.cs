using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip backgroundMusic;

    [Header("Paramètres")]
    [Tooltip("Délai en secondes avant le lancement de la musique")]
    public float delay = 3f;

    [Range(0f, 1f)]
    public float volume = 0.5f;
    public bool loop = true;

    void Start()
    {
        audioSource.clip = backgroundMusic;
        audioSource.volume = 0f;
        audioSource.loop = loop;

        Invoke(nameof(PlayMusic), delay);
    }

    void PlayMusic()
    {
        audioSource.Play();
        StartCoroutine(FadeIn());
    }

    System.Collections.IEnumerator FadeIn()
    {
        float t = 0f;
        float fadeDuration = 2f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, volume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = volume;
    }
}