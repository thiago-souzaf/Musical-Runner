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

    private bool isSustainPressed = false;

    List<Pedal> pedalTimes = new();

    public class Pedal
    {
        public int pressPedalTime;
        public int releasePedalTime;
    }

    [ContextMenu("Convert MIDI to JSON")]
    private void Convert()
    {
        GetPedal();
        ReadFromMIDI();
        WriteJSON();
        Debug.Log("Conversion done!");
    }

    [ContextMenu("Pedal data")]
    private void GetPedal()
    {
        long currentTime = 0;
        MidiFile accompaniment = MidiFile.Read(pathToAccompanimentMIDI);
        Pedal pedal = new();
        foreach (var trackChunk in accompaniment.GetTrackChunks())
        {
            foreach (var midiEvent in trackChunk.Events)
            {
                currentTime += midiEvent.DeltaTime; // Adiciona o tempo delta ao tempo atual

                if (midiEvent is ControlChangeEvent controlChangeEvent)
                {
                    if (controlChangeEvent.ControlNumber == 64)
                    {
                        
                        // É um evento de controle do pedal (CC64)
                        if (controlChangeEvent.ControlValue > 63) // Valor típico para pedal pressionado
                        {
                            pedal = new();
                            pedal.pressPedalTime = (int)currentTime;
                        }
                        else // Valor típico para pedal solto
                        {
                            pedal.releasePedalTime = (int)currentTime;
                            pedalTimes.Add(pedal);
                        }
                    }
                }
            }
        }

        foreach (var pedalTime in pedalTimes)
        {
            Debug.Log($"Pres time: {pedalTime.pressPedalTime} | Release Time: {pedalTime.releasePedalTime}");
        }
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
        File.WriteAllText(Application.dataPath + "/Music/Json Files/" + musicName + "_melody.json", melodyJson);


        // Accompaniment
        NoteInfo[] accompanimentNotes = CreateArray(accNotes);
        string accompanimentJson = JsonHelper.ToJson(accompanimentNotes, true);
        File.WriteAllText(Application.dataPath + "/Music/Json Files/" + musicName + "_accompaniment.json", accompanimentJson);

    }

    NoteInfo[] CreateArray(IEnumerable<Note> notes)
    {
        int length = notes.Count();
        NoteInfo[] notesArray = new NoteInfo[length];
        int index = 0;

        foreach (Note note in notes)
        {
            foreach(Pedal pedalTime in pedalTimes)
            {
                if (note.EndTime >= pedalTime.pressPedalTime && note.EndTime <= pedalTime.releasePedalTime)
                {
                    note.Length = (pedalTime.releasePedalTime - note.Time) - 2;
                }
            }

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
