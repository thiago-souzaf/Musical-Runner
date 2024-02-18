using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    #region Singleton
    public static SettingsManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }
    #endregion

    public AudioMixer audioMixer;

    public UnityEvent<MusicInfo> OnSelectedMusicChange;

    private MusicInfo _selectedMusic;
    public MusicInfo SelectedMusic
    {
        get
        {
            return _selectedMusic;
        }

        set
        {
            _selectedMusic = value;
            OnSelectedMusicChange.Invoke(value);
        }
    }

    public float speedModifier = 1f;
    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    
}
