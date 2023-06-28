using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Animator animator;

    public float moveSpeen = 7f;
    public float jumpForce = 14f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(dirX * moveSpeen, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        }

        UpdateAnimation(dirX);
    }

    private void UpdateAnimation(float dirX)
    {
        rbSprite.flipX = dirX < 0;
        animator.SetBool("running", dirX != 0);
    }
}
