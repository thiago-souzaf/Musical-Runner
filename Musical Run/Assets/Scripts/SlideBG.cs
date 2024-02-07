using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBG : MonoBehaviour
{
    public float speed;

    Vector2 initialPos;

    void Start()
    {
        initialPos = transform.position;
    }

    void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.left);
        if(transform.position.x <= -initialPos.x)
        {
            transform.position = initialPos;
        }
    }
}
