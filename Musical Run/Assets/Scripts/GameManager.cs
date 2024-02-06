using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

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


    public int minNoteLength;
    public int musicBPM;
    public int notesPerBeat;
    public int noteTimeInterval;
    
    public float notesDistance;

}
