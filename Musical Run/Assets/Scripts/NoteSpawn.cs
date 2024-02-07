using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;

public class NoteSpawn : MonoBehaviour
{
    [SerializeField] private GameObject melodyNotePrefab;
    [SerializeField] private GameObject accompanimentNotePrefab;
    [SerializeField] private string pathToMelodyMIDI;
    [SerializeField] private string pathToAccompanimentMIDI;

    [SerializeField] private float[] platformsYPosition;
    [SerializeField] private float accompanimentYPosition;

    private float notesPerMinute;

    private Vector2 melodySpawnPosition;
    private Vector2 accompanimentSpawnPosition;

    private IEnumerable<Note> notes;
    private IEnumerable<Note> accNotes;

    private GameManager gameManager;

    private void Start()
    {
        MidiFile file = MidiFile.Read(pathToMelodyMIDI);
        notes = file.GetNotes();

        MidiFile accompaniment = MidiFile.Read(pathToAccompanimentMIDI);
        accNotes = accompaniment.GetNotes();

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
        foreach (Note note in notes)
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
        foreach (Note note in accNotes)
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
