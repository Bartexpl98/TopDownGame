using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 30f;
    private bool isDashing = false;
    private Vector2 input;
    private Vector2 dashDirection;

    public Rigidbody2D rb;
    public Animator anim;
    public int facingDirection = 1;
    public TrailRenderer trailRenderer;

    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump") && !isDashing)
        {
            print("dashing");
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Vector2 moveVelocity = input * speed;
            if ((moveVelocity.x > 0 && transform.localScale.x < 0) || moveVelocity.x < 0 && transform.localScale.x > 0)
            {
                flip();
            }
            rb.velocity = moveVelocity;
            anim.SetFloat("Horizontal", Mathf.Abs(moveVelocity.x));
            anim.SetFloat("Vertical", Mathf.Abs(moveVelocity.y));
        }
    }

    IEnumerator Dash()
    {
        trailRenderer.emitting = true;
        isDashing = true;

        dashDirection = input.normalized;
        if (dashDirection == Vector2.zero)
        {
            // Fallback to previous direction (optional)
            dashDirection = new Vector2(facingDirection, 0);
        }

        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(0.2f); // dash duration

        isDashing = false;
        trailRenderer.emitting = false;
    }

    void flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}

