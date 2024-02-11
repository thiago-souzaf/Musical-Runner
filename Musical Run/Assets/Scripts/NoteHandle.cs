using UnityEngine;

public class NoteHandle : MonoBehaviour
{
    public int scoreValue;

    [SerializeField] private bool isAccompaniment;

    [HideInInspector] public NoteInfo note;
    [HideInInspector] public bool isLast;

    private AudioSource audioSource;
    private int noteInterval;

    private GameManager gameManager;

    private void Start()
    {
        int octave = note.Octave;
        noteInterval = note.NoteNumber - ((octave + 1) * 12);

        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
        audioSource.clip = gameManager.CNoteClips[octave - 1];

        float newPitch = Mathf.Pow(2, noteInterval / 12.0f);
        audioSource.pitch = newPitch;


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
            gameManager.ResetStreak();
        }
    }

    void PlayNote()
    {
        audioSource.Play();
    }

    void CollectNote()
    {
        if(!isAccompaniment)
            gameManager.IncrementScore(scoreValue);
        PlayNote();
        DestroyNote();
        float noteDuration = 60f * note.Length / (gameManager.musicBPM * gameManager.beatInterval);
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

        if (isLast)
        {
            gameManager.FinishLevel();
        }
        Destroy(gameObject, 2f);
    }
}
