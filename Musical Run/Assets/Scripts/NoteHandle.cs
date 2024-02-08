using UnityEngine;

public class NoteHandle : MonoBehaviour
{
    public NoteInfo note;

    private AudioSource audioSource;
    private int noteInterval;

    private GameManager gameManager;

    private void Start()
    {
        int octave = note.Octave;
        noteInterval = note.NoteNumber - ((octave + 1) * 12);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = NoteManager.instance.CNoteClips[octave - 1];

        float newPitch = Mathf.Pow(2, noteInterval / 12.0f);
        audioSource.pitch = newPitch;

        gameManager = GameManager.Instance;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectNote();
        }
        else if (other.CompareTag("OuterBounds"))
        {
            DestroyNote();
        }
    }

    void PlayNote()
    {
        audioSource.Play();
    }

    void CollectNote()
    {
        PlayNote();
        DestroyNote();

        float noteDuration = 60f * note.Length / (gameManager.musicBPM * gameManager.notesPerBeat * gameManager.minNoteLength);
        Invoke(nameof(StopPlay), noteDuration);
    }

    void StopPlay()
    {
        audioSource.Stop();
    }

    void DestroyNote()
    {
        if(TryGetComponent(out SpriteRenderer sr)){
            sr.enabled = false;
        }
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 2f);
    }
}
