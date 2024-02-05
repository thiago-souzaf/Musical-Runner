using UnityEngine;

public class CountInterval : MonoBehaviour
{
    private float hitTimeDelta;

    private void Start()
    {
        hitTimeDelta = 0f;
    }

    private void Update()
    {
        hitTimeDelta += Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            Debug.Log(hitTimeDelta.ToString());
            hitTimeDelta = 0;
        }
    }
}
