using TMPro;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject panel;
    

    public void ShowLevelCompletePanel(int finalScore)
    {
        scoreText.text = finalScore.ToString();
        panel.SetActive(true);
    }
}
