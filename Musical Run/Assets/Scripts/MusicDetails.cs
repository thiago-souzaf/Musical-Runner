using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicDetails : MonoBehaviour
{
    [SerializeField] Button confirmButton;

    [SerializeField] GameObject selectedMusicInfo;
    [SerializeField] GameObject message;

    [SerializeField] TextMeshProUGUI musicName;
    [SerializeField] TextMeshProUGUI composer;
    [SerializeField] TextMeshProUGUI year;
    [SerializeField] TextMeshProUGUI key;
    [SerializeField] TextMeshProUGUI highScore;

    private void Start()
    {
        MusicSelection.Instance.OnSelectedMusicChange.AddListener(ChangeDetails);
        selectedMusicInfo.SetActive(false);
        message.SetActive(true);
        confirmButton.interactable = false;
    }

    public void ChangeDetails(MusicInfo info)
    {
        musicName.text = info.musicName;
        composer.text = info.composer;
        year.text = info.year;
        key.text = info.key;
        highScore.text = info.highScore.ToString();

        selectedMusicInfo.SetActive(true);
        message.SetActive(false);

        confirmButton.interactable = true;
    }

}
