using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI completionMessageText;
    [SerializeField] GameObject newHighscoreText;
    [SerializeField] GameObject panel;
    [SerializeField] Slider[] scoreFillers;

    private int m_finalScore;
    private int m_MaxScore;
    private float m_StarsFilled;
    private int m_StarsTotal;

    public void ShowLevelCompletePanel(int finalScore)
    {
        m_finalScore = finalScore;
        m_StarsTotal = scoreFillers.Length;
        m_MaxScore = (GameManager.Instance.numberOfMelodyNotes - 21) * 40 + 420;
        m_StarsFilled = finalScore * m_StarsTotal / (float) m_MaxScore;

        scoreText.text = finalScore.ToString();
        completionMessageText.text = GetCompletionText();

        FillScore();
        CheckHighScore();
        panel.SetActive(true);

    }

    private void FillScore()
    {
        for (int i = 0; i < m_StarsTotal; i++)
        {
            scoreFillers[i].value = Mathf.Clamp01(m_StarsFilled - i);
        }
    }

    private string GetCompletionText()
    {
        string completionText = m_StarsFilled >= 2 ? "Excellent" : m_StarsFilled >= 1 ? "Good" : "Not Bad";
        return completionText;
    }

    private void CheckHighScore()
    {
        string musicName = SettingsManager.Instance.SelectedMusic.musicName;
        string intKey = musicName + "_highscore";
        int currentHighscore = PlayerPrefs.GetInt(intKey, 0);

        if(m_finalScore > currentHighscore)
        {
            // Update highscore
            PlayerPrefs.SetInt(intKey, m_finalScore);
            newHighscoreText.SetActive(true);
        }
    }
}
