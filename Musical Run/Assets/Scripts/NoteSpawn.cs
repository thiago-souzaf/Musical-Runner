using UnityEngine;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class NoteSpawn : MonoBehaviour
{
    [SerializeField] private GameObject notePrefab;
    public Vector2 startSpawnPosition;

    public float[] platformsYPosition = new float[] { -3, 0, 3};

    private Vector2 currentSpawnPosition;

    private IEnumerable<Note> notes;

    private void Start()
    {
        MidiFile file = MidiFile.Read("Assets/MIDI Files/Fur_elise_melody.mid");
        
        notes = file.GetNotes();

        currentSpawnPosition = startSpawnPosition;

        SpawnNotes();
    }

    void SpawnNotes()
    {
        int index = 0;
        int previousNoteHeight = 0;

        foreach (Note note in notes)
        {
            index = note.NoteNumber > previousNoteHeight ? index+1 : note.NoteNumber < previousNoteHeight ? index-1 : index;
            index = Mathf.Clamp(index, 0, platformsYPosition.Length-1);

            currentSpawnPosition.y = platformsYPosition[index];
            currentSpawnPosition.x = (GameManager.Instance.notesDistance * note.Time) / GameManager.Instance.noteTimeInterval;

            NoteInfo ni_note = Instantiate(notePrefab, currentSpawnPosition, Quaternion.identity, transform).GetComponent<NoteInfo>();
            ni_note.note = note;

            previousNoteHeight = note.NoteNumber;
        }
    }
}
