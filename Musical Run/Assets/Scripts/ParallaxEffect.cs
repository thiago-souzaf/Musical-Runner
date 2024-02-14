using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float speedDecreaseRate = 0.7f;
    [SerializeField] float xPositionWrap;
    [SerializeField] Transform[] bgTransforms;

    private Vector2[] bgStartPositions;
    void Start()
    {
        bgStartPositions = new Vector2[bgTransforms.Length];
        for (int i = 0; i < bgTransforms.Length; i++)
        {
            bgStartPositions[i] = bgTransforms[i].position;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0;i < bgTransforms.Length;i++)
        {
            float bgSpeed = speed * Mathf.Pow(speedDecreaseRate,i);
            bgTransforms[i].Translate(bgSpeed * Time.deltaTime * Vector2.left);

            if (bgTransforms[i].position.x <= xPositionWrap) { bgTransforms[i].position = bgStartPositions[i]; }
        }
    }
}
