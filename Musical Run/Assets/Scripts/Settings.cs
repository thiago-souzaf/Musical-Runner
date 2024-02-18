using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumePercentage;

    [SerializeField] private TextMeshProUGUI selectedSpeed;

    [SerializeField] float[] speedValues;
    private Dictionary<float, string> speedOptions;
    
    private int currentSpeedIndex;

    private SettingsManager settingsManager;
    
    private void Start()
    {
        settingsManager = SettingsManager.Instance;

        // Volume
        settingsManager.audioMixer.GetFloat("MasterVolume", out float volumeDB);
        float percentageVolume = Mathf.Pow(10, (volumeDB / 20)) * 100;
        volumeSlider.value = Mathf.RoundToInt(percentageVolume / 20);
        int percentage = (int)volumeSlider.value * 20;
        volumePercentage.text = percentage + "%";

        // Speed
        speedOptions = new Dictionary<float, string>()
        {
            {speedValues[0], "Slow" },
            {speedValues[1], "Normal" },
            {speedValues[2], "Fast" }
        };

        float speed = 1f;
        if (speedOptions.ContainsKey(settingsManager.speedModifier))
            speed = settingsManager.speedModifier;

        selectedSpeed.text = speedOptions[speed];
        currentSpeedIndex = speedOptions.Keys.ToList().IndexOf(speed);

    }
    public void ChangeVolume(int amount)
    {
        float previousVolume = volumeSlider.value;
        previousVolume += amount;
        volumeSlider.value = previousVolume;

        // Audio mixing attenuation min is -80dB
        float clampedVolume = Mathf.Clamp(previousVolume, 0.00000005f, 5);
        float newVolume = 20 * Mathf.Log10((clampedVolume / 5f));
        settingsManager.ChangeVolume(newVolume);

        // Change the text
        int percentage = (int)volumeSlider.value * 20;
        volumePercentage.text = percentage + "%";
    }

    public void SwitchSpeed(int amount)
    {
        currentSpeedIndex += amount;
        currentSpeedIndex = Mathf.Clamp(currentSpeedIndex, 0, speedOptions.Count - 1);

        selectedSpeed.text = speedOptions.ElementAt(currentSpeedIndex).Value;

        settingsManager.speedModifier = speedOptions.ElementAt(currentSpeedIndex).Key;
    }

}
