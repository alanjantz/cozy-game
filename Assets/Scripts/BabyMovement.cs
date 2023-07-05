using UnityEngine;

public class BabyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Animator animator;

    public float moveSpeed = 7f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(dirX * moveSpeed, rb.velocity.y);

        UpdateAnimation(dirX);
    }

    private void UpdateAnimation(float dirX)
    {
        if (dirX == 0)
            animator.SetBool("running", false);
        else
        {
            animator.SetBool("running", true);
            rbSprite.flipX = dirX > 0;
        }
    }
}
