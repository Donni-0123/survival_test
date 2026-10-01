
using UnityEngine;
using UnityEngine.UI;

public class SettingsAudio : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager не знайдено!");
            return;
        }

        musicVolumeSlider.minValue = 0f;
        musicVolumeSlider.maxValue = 1f;
        musicVolumeSlider.wholeNumbers = false;

        // Встановлюємо збережену гучність
        musicVolumeSlider.SetValueWithoutNotify(
            AudioManager.Instance.MusicVolume
        );

        // Підключаємо зміну гучності
        musicVolumeSlider.onValueChanged.AddListener(
            OnMusicVolumeChanged
        );
    }

    private void OnDestroy()
    {
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.RemoveListener(
                OnMusicVolumeChanged
            );
        }
    }

    private void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }
}