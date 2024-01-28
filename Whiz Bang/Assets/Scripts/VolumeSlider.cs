using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public AudioManager audioManager;

    void Start()
    {
        // Find the AudioManager instance in the scene
        audioManager = AudioManager.instance;

        // Set the slider value to the current master volume
        slider.value = audioManager.masterVolume;

        // Add a listener to the slider's value change event
        slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    // Method called when the slider's value changes
    public void OnSliderValueChanged()
    {
        // Update the master volume in the AudioManager
        audioManager.SetMasterVolume(slider.value);
    }
}
