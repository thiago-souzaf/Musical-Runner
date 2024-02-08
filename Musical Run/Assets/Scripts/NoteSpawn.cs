using UnityEngine;
using System.Collections;
using System.IO;

public class NoteSpawn : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject melodyNotePrefab;
    [SerializeField] private GameObject accompanimentNotePrefab;

    [Header("Json Files")]
    [SerializeField] private string pathToMelodyJson;
    [SerializeField] private string melodyJson;

    [SerializeField] private string pathToAccompanimentJson;
    [SerializeField] private string accompanimentJson;

    [Header("Y Axis spawn position")]
    [SerializeField] private float[] platformsYPosition;
    [SerializeField] private float accompanimentYPosition;

    private float notesPerMinute;
    private float musicStartTime;

    private NoteInfo[] notes;
    private NoteInfo[] accNotes;

    private GameManager gameManager;

    private void Start()
    {
        notes = JsonHelper.FromJson<NoteInfo>(melodyJson);
        accNotes = JsonHelper.FromJson<NoteInfo>(accompanimentJson);

        musicStartTime = Time.time;

        gameManager = GameManager.Instance;
        notesPerMinute = gameManager.notesPerBeat * gameManager.musicBPM;

        StartCoroutine(SpawnMelodyNotes());
        StartCoroutine(SpawnAccompaniment());
    }

    IEnumerator SpawnMelodyNotes()
    {
        int index = 0;
        int previousNoteHeight = 0;

        Vector2 melodySpawnPos;
        melodySpawnPos.x = transform.position.x;

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
            SpawnNote(note, melodyNotePrefab, melodySpawnPos);
        }
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

    private void SpawnNote(NoteInfo note, GameObject notePrefab, Vector2 spawnPosition)
    {
        NoteHandle nh_note = Instantiate(notePrefab, spawnPosition, Quaternion.identity, transform).GetComponent<NoteHandle>();
        nh_note.note = note;
    }

    private float GetWaitTime(float startTime)
    {
        float timeToPlay = startTime * 60 / (gameManager.noteTimeInterval * notesPerMinute) + musicStartTime;
        float waitTime = timeToPlay - Time.time;
        return waitTime;
    }
}
