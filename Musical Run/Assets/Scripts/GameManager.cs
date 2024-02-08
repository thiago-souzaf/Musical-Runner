using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public int noteTimeInterval;
    public int musicBPM;
    public int notesPerBeat;

    [Tooltip("Distance in units between 2 sequencial notes")]
    public float notesDistance;

    public AudioClip[] CNoteClips;


    // Score Manager

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Slider noteStreakSlider;
    [SerializeField] private int streakSequence = 7;

    private int m_Score;
    private int noteStreak;
    private int multiplyValue;

    private void Start()
    {
        ResetStreak();
    }

    public void IncrementScore(int score)
    {
        m_Score += score * multiplyValue;
        scoreText.text = "Score: " + m_Score;
        noteStreak++;

        if (noteStreak >= streakSequence && multiplyValue < 4)
        {
            IncrementMultiplier();
            noteStreak = 0;
        }

        noteStreakSlider.value = noteStreak;
    }

    public void ResetStreak()
    {
        noteStreak = 0;
        noteStreakSlider.value = noteStreak;
        multiplyValue = 1;
        multiplierText.text = "x" + multiplyValue;
    }

    void IncrementMultiplier()
    {
        multiplyValue++;
        multiplierText.text = "x" + multiplyValue;
    }
}
