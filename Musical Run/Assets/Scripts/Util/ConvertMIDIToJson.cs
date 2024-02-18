using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Core;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class ConvertMIDIToJson : MonoBehaviour
{
    [SerializeField] string pathToMelodyMIDI;
    [SerializeField] string pathToAccompanimentMIDI;
    [SerializeField] string musicName;

    private IEnumerable<Note> notes;
    private IEnumerable<Note> accNotes;

    [ContextMenu("Convert MIDI to JSON")]
    private void Convert()
    {
        ReadFromMIDI();
        WriteJSON();
        Debug.Log("Conversion done!");
    }

    void ReadFromMIDI()
    {
        MidiFile file = MidiFile.Read(pathToMelodyMIDI);
        notes = file.GetNotes();

        MidiFile accompaniment = MidiFile.Read(pathToAccompanimentMIDI);
        accNotes = accompaniment.GetNotes();
    }

    void WriteJSON()
    {
        // Melody
        NoteInfo[] melodyNotes = CreateArray(notes);
        string melodyJson = JsonHelper.ToJson(melodyNotes, true);
        File.WriteAllText(Application.dataPath + "/Audio/Json Files/" + musicName + "_melody.json", melodyJson);

        // Accompaniment
        NoteInfo[] accompanimentNotes = CreateArray(accNotes);
        string accompanimentJson = JsonHelper.ToJson(accompanimentNotes, true);
        File.WriteAllText(Application.dataPath + "/Audio/Json Files/" + musicName + "_accompaniment.json", accompanimentJson);

    }

    NoteInfo[] CreateArray(IEnumerable<Note> notes)
    {
        int length = notes.Count();
        NoteInfo[] notesArray = new NoteInfo[length];
        int index = 0;

        foreach (Note note in notes)
        {
            NoteInfo currentNote = new()
            {
                Octave = note.Octave,
                NoteNumber = note.NoteNumber,
                Length = note.Length,
                Time = note.Time,
                EndTime = note.EndTime,
                Velocity = note.Velocity,
                OffVelocity = note.OffVelocity,
            };

            notesArray[index] = currentNote;
            index++;
        }

        return notesArray;
    }

    
}
