using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager instance;

    // Volume level
    public float masterVolume = 1.0f;

    // PlayerPrefs keys
    private const string MasterVolumeKey = "MasterVolume";

    // List to store all audio sources
    private List<AudioSource> allAudioSources = new List<AudioSource>();

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        // Load saved volume
        if (PlayerPrefs.HasKey(MasterVolumeKey))
        {
            masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey);
            UpdateVolume();
        }

        StartCoroutine(ScanForAudioSources());
    }

    IEnumerator<WaitForSeconds> ScanForAudioSources()
    {
        while (true)
        {
            // Scan the scene for audio sources
            ScanSceneForAudioSources();

            // Wait for a short duration before scanning again
            yield return new WaitForSeconds(2.0f);
        }
    }

    void ScanSceneForAudioSources()
    {
        // Clear the list of audio sources
        allAudioSources.Clear();

        // Find all audio sources in the scene
        AudioSource[] sceneAudioSources = FindObjectsOfType<AudioSource>();
        allAudioSources.AddRange(sceneAudioSources);

        // Find audio sources from instantiated prefabs
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            AudioSource[] prefabAudioSources = rootGameObject.GetComponentsInChildren<AudioSource>(true);
            allAudioSources.AddRange(prefabAudioSources);
        }

        // Update volume
        UpdateVolume();
    }

    // Set master volume
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateVolume();
        // Save volume
        PlayerPrefs.SetFloat(MasterVolumeKey, volume);
        PlayerPrefs.Save();
    }

    // Update volume for all audio sources
    private void UpdateVolume()
    {
        foreach (var audioSource in allAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.volume = masterVolume;
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ScanSceneForAudioSources();
    }
}
