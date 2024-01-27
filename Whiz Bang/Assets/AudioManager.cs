using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager instance;

    // Reference to audio sources
    public AudioSource[] audioSources;

    // Volume level
    public float masterVolume = 1.0f;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Persist across scenes
        DontDestroyOnLoad(gameObject);

        // Get all audio sources
        audioSources = FindObjectsOfType<AudioSource>();
    }

    // Set master volume
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;

        // Update volume for all audio sources
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = masterVolume;
        }
    }
}