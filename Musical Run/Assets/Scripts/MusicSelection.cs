using UnityEngine;
using UnityEngine.Events;

public class MusicSelection : MonoBehaviour
{
    #region Singleton
    public static MusicSelection Instance { get; private set; }


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

}
