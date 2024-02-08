using UnityEngine;

public class ResetPlatformPositions : MonoBehaviour
{
    Vector2 initialPos;
    public float limitToWrap;

    private void Start()
    {
        initialPos = transform.position;
    }
    private void FixedUpdate()
    {
        if(transform.position.x <= limitToWrap)
        {
            transform.position = initialPos;
        }
    }
}
