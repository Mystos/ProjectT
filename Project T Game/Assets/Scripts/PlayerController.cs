using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isGrounded;
    public Transform groundCheckLeft;
    public Transform groundCheckRight;

    public Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;

    public bool isJumping = false;
    public int doubleJump = 1;
    public bool isDoubleJumping = false;
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position); // Dessine une ligne entre les deux transforme

        if (isGrounded)
        {
            doubleJump = 1;
        }

        animator.SetBool("IsGrounded", isGrounded);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

        // Double saut
        if (Input.GetButtonDown("Jump") && !isGrounded && doubleJump >= 1)
        {
            isDoubleJumping = true;
            doubleJump--;
        }

        animator.SetBool("IsDoubleJumping", isDoubleJumping);
        animator.SetInteger("JumpCount", doubleJump);

    }

    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        MovePlayer(horizontalMovement);

        Flip(rb.velocity.x);

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("xSpeed", characterVelocity);
        animator.SetFloat("ySpeed", rb.velocity.y);
    }

    void MovePlayer(float horizontalMovement)
    {
        Vector3 targetVelocity = new Vector2(horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);


        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }
        if (isDoubleJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isDoubleJumping = false;

        }
    }

    void Flip(float velocity)
    {

        if(velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if(velocity < -0.1f)
        {
            spriteRenderer.flipX = true;

        }
    }
}
