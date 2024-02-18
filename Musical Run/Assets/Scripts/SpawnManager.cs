using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private string melodyTag;
    [SerializeField] private string accompanimentTag;

    [Header("Json Files")]
    [SerializeField] private TextAsset melodyJSON;
    [SerializeField] private TextAsset accompanimentJSON;

    [Header("Y Axis spawn position")]
    [SerializeField] private float[] platformsYPosition;
    [SerializeField] private float accompanimentYPosition;

    private float musicStartTime;

    private NoteInfo[] notes;
    private NoteInfo[] accNotes;

    private GameManager gameManager;
    private NotesPooler notesPooler;

    private void Start()
    {
        notes = JsonHelper.FromJson<NoteInfo>(melodyJSON.text);
        accNotes = JsonHelper.FromJson<NoteInfo>(accompanimentJSON.text);

        musicStartTime = Time.time;

        gameManager = GameManager.Instance;
        notesPooler = NotesPooler.instance;

        StartCoroutine(SpawnMelodyNotes());
        StartCoroutine(SpawnAccompaniment());
    }

    IEnumerator SpawnMelodyNotes()
    {
        int index = 0;
        int previousNoteHeight = 0;

        Vector2 melodySpawnPos;
        melodySpawnPos.x = transform.position.x;

        NoteHandle noteSpawned = null;
        foreach (NoteInfo note in notes)
        {
            // Sets the y position based on previous note height
            index = note.NoteNumber > previousNoteHeight ? index+1 : note.NoteNumber < previousNoteHeight ? index-1 : index;
            if (index == platformsYPosition.Length)
                index -= 2;
            else if (index == -1)
                index += 2;
            melodySpawnPos.y = platformsYPosition[index];
            previousNoteHeight = note.NoteNumber;

            // Wait for spawn
            yield return new WaitForSeconds(GetWaitTime(note.Time));

            // Instantiate after waiting
            noteSpawned = SpawnNote(note, melodyTag, melodySpawnPos);
        }

        noteSpawned.isLast = true;
    }

    IEnumerator SpawnAccompaniment()
    {
        Vector2 accompanimentSpawnPosition = new(transform.position.x, accompanimentYPosition);

        foreach (NoteInfo note in accNotes)
        {
            // Wait for spawn
            yield return new WaitForSeconds(GetWaitTime(note.Time));

            // Instantiate after waiting
            SpawnNote(note, accompanimentTag, accompanimentSpawnPosition);
        }
    }

    private NoteHandle SpawnNote(NoteInfo note, string dictTag, Vector2 spawnPosition)
    {
        GameObject noteSpawned = notesPooler.SpawnFromPool(dictTag, spawnPosition, Quaternion.identity, note);
        NoteHandle nh_note = noteSpawned.GetComponent<NoteHandle>();

        return nh_note;
    }

    private float GetWaitTime(float startTime)
    {
        float timeToPlay = startTime * 60 / (gameManager.beatInterval * gameManager.musicBPM) + musicStartTime;

        float waitTime = timeToPlay - Time.time;

        return waitTime;
    }

    private void OnLevelWasLoaded(int level)
    {
        MusicInfo selectedMusic = SettingsManager.Instance.SelectedMusic;

        melodyJSON = selectedMusic.melodyJson;
        accompanimentJSON = selectedMusic.accompanimentJson;
    }
}
