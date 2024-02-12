using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
            return;
        }

        Destroy(gameObject);
    }
    #endregion

    [Header("Music information")]
    public int beatInterval;
    public int musicBPM;
    public int numberOfMelodyNotes;

    [Space]
    [Tooltip("Distance in units between 2 beats")]
    public float beatsDistance;

    [Header("Audio clips for piano sounds (only C notes of each octave)")]
    public AudioClip[] CNoteClips;

    [Space(50f)]

    #region Score Management
    [Header("Score management")]
    [Tooltip("Number of notes needed to increase multiply value")]
    [SerializeField] private int streakSequence = 7;
    [Tooltip("Max multiply value")]
    [SerializeField] private int maxMultiply = 4;
    [Space]
    public UnityEvent<int> onLevelFinish;
    [Space]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Slider noteStreakSlider;

    private int m_Score;
    private int noteStreak;
    private int multiplyValue;

    public void IncrementScore(int score)
    {
        m_Score += score * multiplyValue;
        scoreText.text = "Score: " + m_Score;
        noteStreak++;

        if (noteStreak >= streakSequence && multiplyValue < maxMultiply)
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
    private void ResetScore()
    {
        m_Score = 0;
        ResetStreak();
    }

    #endregion

    public void FinishLevel()
    {
        onLevelFinish.Invoke(m_Score);
    }

    private void Start()
    {
        ResetScore();
    }

    private void OnLevelWasLoaded(int level)
    {
        ResetScore();
    }
}
