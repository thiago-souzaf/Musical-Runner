using System.Collections;
using UnityEngine;

public class NoteHandle : MonoBehaviour, IPooledObject
{
    [SerializeField] private int scoreValue;
    [SerializeField] private bool isAccompaniment;
    [SerializeField] private GameObject collectedParticleSystem;

    [HideInInspector] public NoteInfo note;
    [HideInInspector] public bool isLast;

    private AudioSource audioSource;
    private int noteInterval;
    private float noteDuration;

    private GameManager gameManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
    }

    public void OnObjectSpawn()
    {
        int octave = note.Octave;
        noteInterval = note.NoteNumber - ((octave + 1) * 12);

        audioSource.clip = gameManager.CNoteClips[octave - 1];

        noteDuration = 60f * note.Length / (gameManager.musicBPM * gameManager.beatInterval);

        float newPitch = Mathf.Pow(2, noteInterval / 12.0f);
        audioSource.pitch = newPitch;

        if (TryGetComponent(out SpriteRenderer sr))
            sr.enabled = true;

        GetComponent<BoxCollider2D>().enabled = true;

        if (collectedParticleSystem != null)
            collectedParticleSystem.SetActive(false);
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

        if (collectedParticleSystem != null)
            collectedParticleSystem.SetActive(true);

        Invoke(nameof(StopNote), noteDuration);

    }

    void StopNote()
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

        StartCoroutine(DeactivateNote(noteDuration));
    }

    IEnumerator DeactivateNote(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    } 
}
