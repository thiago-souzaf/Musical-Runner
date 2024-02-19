using UnityEngine;

public class NoteHandle : MonoBehaviour, IPooledObject
{
    [HideInInspector] public NoteInfo note;

    protected AudioSource audioSource;
    protected int noteInterval;
    protected float noteDuration;

    protected GameManager gameManager;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
    }

    virtual public void OnObjectSpawn()
    {
        int octave = note.Octave;
        noteInterval = note.NoteNumber - ((octave + 1) * 12);

        audioSource.clip = gameManager.CNoteClips[octave - 1];

        noteDuration = 60f * note.Length / (gameManager.musicBPM * gameManager.beatInterval);

        float newPitch = Mathf.Pow(2, noteInterval / 12.0f);
        audioSource.pitch = newPitch;

        GetComponent<BoxCollider2D>().enabled = true;
    }
    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectNote();
        }
    }

    protected virtual void CollectNote()
    {
        audioSource.Play();
        DestroyNote();
    }

    virtual protected void DestroyNote()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke(nameof(DeactivateNote), noteDuration);
    }

    void DeactivateNote()
    {
        audioSource.Stop();
        gameObject.SetActive(false);
    } 
}
