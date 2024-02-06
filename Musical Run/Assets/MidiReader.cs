using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MidiReader : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        MidiFile file = MidiFile.Read("Assets/Fur_Elise.mid");
        IEnumerable<Note> notes = file.GetNotes();
        foreach (Note note in notes)
        {
            Debug.Log("Note name: " + note.NoteName);
            Debug.Log("Note lenght: " + note.Length);
        }
    }

}
