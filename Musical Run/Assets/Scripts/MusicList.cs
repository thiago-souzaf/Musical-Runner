using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicList : MonoBehaviour
{
    public List<MusicInfo> musicInfos;
    [SerializeField] private MusicButton musicButtonPrefab;

    private void Start()
    {
        CreateButtonsFromList();
    }

    private void CreateButtonsFromList()
    {
        foreach (MusicInfo musicInfo in musicInfos)
        {
            CreateButton(musicInfo);
        }
    }

    private void CreateButton(MusicInfo musicInfo)
    {
        MusicButton btn = Instantiate(musicButtonPrefab, transform);

        btn.musicName.text = musicInfo.musicName;
        btn.composerName.text = musicInfo.composer;
        btn.time.text = musicInfo.totalTime;

        // Sets on clik event to change the selected music
        Button c_btn = btn.GetComponent<Button>();
        c_btn.onClick.AddListener(() => MusicSelection.Instance.SelectedMusic = musicInfo);
    }

}
