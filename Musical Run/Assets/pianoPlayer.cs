using UnityEngine;

public class pianoPlayer : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {
        int note = -101; // invalid value to detect when note is pressed
        if (Input.GetKeyDown("a")) note = -5;  // E
        if (Input.GetKeyDown("s")) note = -4;  // F
        if (Input.GetKeyDown("d")) note = -2;  // G
        if (Input.GetKeyDown("f")) note =  0;  // A

        if (Input.GetKeyDown("h")) note = 2;  // B
        if (Input.GetKeyDown("j")) note = 3;  // C
        if (Input.GetKeyDown("k")) note = 5;  // D
        if (Input.GetKeyDown("l")) note = 7;  // E

        if (note >= -100)
        { // if some key pressed...
            audioSource.pitch = Mathf.Pow(2, note / 12.0f);
            audioSource.Play();
        }
    }
}
