using UnityEngine;

public class MelodyNoteHandle : NoteHandle
{
    [SerializeField] private int scoreValue;
    [SerializeField] private GameObject collectedParticleSystem;
    [SerializeField] private GameObject fakeLight;

    [HideInInspector] public bool isLast;

    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
    }

    override public void OnObjectSpawn()
    {
        base.OnObjectSpawn();

        sr.enabled = true;
        fakeLight.SetActive(true);
        collectedParticleSystem.SetActive(false);
        
    }

    override protected void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("OuterBounds"))
        {
            DestroyNote();
        }
    }

    override protected void CollectNote()
    {
        base.CollectNote();
        gameManager.IncrementScore(scoreValue);
        collectedParticleSystem.SetActive(true);
    }

    override protected void DestroyNote()
    {
        base.DestroyNote();
        sr.enabled = false;
        fakeLight.SetActive(false);

        if (isLast)
        {
            gameManager.FinishLevel();
        }
    }
}
