using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject melodyNotePrefab;
    [SerializeField] private GameObject accompanimentNotePrefab;

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

    private void Start()
    {
        notes = JsonHelper.FromJson<NoteInfo>(melodyJSON.text);
        accNotes = JsonHelper.FromJson<NoteInfo>(accompanimentJSON.text);

        musicStartTime = Time.time;

        gameManager = GameManager.Instance;

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
            noteSpawned = SpawnNote(note, melodyNotePrefab, melodySpawnPos);
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
            SpawnNote(note, accompanimentNotePrefab, accompanimentSpawnPosition);
        }
    }

    private NoteHandle SpawnNote(NoteInfo note, GameObject notePrefab, Vector2 spawnPosition)
    {
        NoteHandle nh_note = Instantiate(notePrefab, spawnPosition, Quaternion.identity, transform).GetComponent<NoteHandle>();
        nh_note.note = note;
        return nh_note;
    }

    private float GetWaitTime(float startTime)
    {
        float timeToPlay = startTime * 60 / (gameManager.beatInterval * gameManager.musicBPM) + musicStartTime;

        float waitTime = timeToPlay - Time.time;

        return waitTime;
    }
}
