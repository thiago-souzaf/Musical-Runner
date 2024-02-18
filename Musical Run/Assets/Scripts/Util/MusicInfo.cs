using UnityEngine;

[CreateAssetMenu(fileName = "New Music Info", menuName = "Music Info")]
public class MusicInfo : ScriptableObject
{
    [Header("Music info - Selection Menu")]
    public string musicName;
    public string composer;
    public string year;
    public string key;
    public string totalTime;

    [Header("Music info - Create scene")]
    public int beatInterval;
    public int musicBPM;
    public int numberOfMelodyNotes;
    public float beatsDistance;
    public TextAsset melodyJson;
    public TextAsset accompanimentJson;

}
