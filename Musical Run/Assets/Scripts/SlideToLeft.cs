using UnityEngine;

public class SlideToLeft : MonoBehaviour
{
    private int notesPerBeat;
    private float notesDistance;
    private int bpm;
    private float speed;


    private void Start()
    {
        GameManager gameManager = GameManager.Instance;

        notesPerBeat = gameManager.notesPerBeat;
        notesDistance = gameManager.notesDistance;
        bpm = gameManager.musicBPM;

        speed = (float)(notesPerBeat * notesDistance * bpm) / 60f;
        
    }
    private void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.left);
    }
}
