using UnityEngine;

public class SlideToLeft : MonoBehaviour
{
    private float notesDistance;
    private int bpm;
    private float speed;

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;

        notesDistance = gameManager.beatsDistance;
        bpm = gameManager.musicBPM;

        speed = (float) (notesDistance * bpm) / 60f;
        
    }
    private void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.left);
    }
}
