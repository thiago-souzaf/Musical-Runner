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

    private Vector2 melodySpawnPosition;
    private Vector2 accompanimentSpawnPosition;

    private NoteInfo[] notes;
    private NoteInfo[] accNotes;

    private GameManager gameManager;

    private void Start()
    {
        notes = JsonHelper.FromJson<NoteInfo>(melodyJson);
        accNotes = JsonHelper.FromJson<NoteInfo>(accompanimentJson);

        gameManager = GameManager.Instance;
        notesPerMinute = gameManager.notesPerBeat * gameManager.musicBPM;

        accompanimentSpawnPosition = new Vector2(transform.position.x, accompanimentYPosition);

        StartCoroutine(SpawnMelodyNotes());
        StartCoroutine(SpawnAccompaniment());
    }

    IEnumerator SpawnMelodyNotes()
    {
        int index = 0;
        int previousNoteHeight = 0;

        melodySpawnPosition.x = transform.position.x;
        foreach (NoteInfo note in notes)
        {
            // Sets the y position based on previous note height
            index = note.NoteNumber > previousNoteHeight ? index+1 : note.NoteNumber < previousNoteHeight ? index-1 : index;
            if (index == platformsYPosition.Length)
                index -= 2;
            else if (index == -1)
                index += 2;
            melodySpawnPosition.y = platformsYPosition[index];
            previousNoteHeight = note.NoteNumber;

            // Wait for spawn
            float timeToPlay = note.Time * 60 / (gameManager.noteTimeInterval * notesPerMinute);
            float waitTime = timeToPlay - Time.time;
            yield return new WaitForSeconds(waitTime);

            // Instantiate after waiting
            NoteHandle ni_note = Instantiate(melodyNotePrefab, melodySpawnPosition, Quaternion.identity, transform).GetComponent<NoteHandle>();
            ni_note.note = note;
        }
    }

    IEnumerator SpawnAccompaniment()
    {
        foreach (NoteInfo note in accNotes)
        {
            // Wait for spawn
            float timeToPlay = note.Time * 60 / (gameManager.noteTimeInterval * notesPerMinute);
            float waitTime = timeToPlay - Time.time;
            yield return new WaitForSeconds(waitTime);

            // Instantiate after waiting
            NoteHandle nh_note = Instantiate(accompanimentNotePrefab, accompanimentSpawnPosition, Quaternion.identity, transform).GetComponent<NoteHandle>();
            nh_note.note = note;
        }
    }
}
