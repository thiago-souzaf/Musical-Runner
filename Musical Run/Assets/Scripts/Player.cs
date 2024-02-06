using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float jumpForce;
    public float timeToEnableCollision;

    private int playerLayerIndex;
    private int currentPlatformIndex;
    private Rigidbody2D rb;

    private PlayerControls inputActions;
    private InputAction up;
    private InputAction down;

    private Animator anim;

    private void Awake()
    {
        inputActions = new();

        up = inputActions.Player.Jump;
        up.performed += ctx => GetUp();

        down = inputActions.Player.Down;
        down.performed += ctx => GetDown();
    }

    private void OnEnable()
    {
        up.Enable();
        down.Enable();
    }

    private void OnDisable()
    {
        up.Disable();
        down.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLayerIndex = gameObject.layer;
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        anim.SetFloat("verticalVelocity", rb.velocity.y);
    }
    public void GetUp()
    {
        if(currentPlatformIndex >= 0)
        {
            rb.velocity = new(rb.velocity.x, jumpForce);
        }
    }

    public void GetDown()
    {
        if(currentPlatformIndex > 0)
            StartCoroutine(DisableCollision(currentPlatformIndex));
    }

    IEnumerator DisableCollision(int platformLayerToDisable)
    {
        Physics2D.IgnoreLayerCollision(playerLayerIndex, platformLayerToDisable);
        yield return new WaitForSeconds(timeToEnableCollision);
        Physics2D.IgnoreLayerCollision(playerLayerIndex, platformLayerToDisable, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentPlatformIndex = collision.gameObject.layer;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentPlatformIndex = -1;
        }
    }
}
