
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource musicSource;
    private const string VolumeKey = "MusicVolume";

    public float MusicVolume { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();

        MusicVolume = PlayerPrefs.GetFloat(VolumeKey, 0.5f);
        musicSource.volume = MusicVolume;
        musicSource.loop = true;
        musicSource.playOnAwake = true;

        if (!musicSource.isPlaying && musicSource.clip != null)
        {
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = Mathf.Clamp01(volume);

        if (musicSource != null)
        {
            musicSource.volume = MusicVolume;
        }

        PlayerPrefs.SetFloat(VolumeKey, MusicVolume);
        PlayerPrefs.Save();
    }
}