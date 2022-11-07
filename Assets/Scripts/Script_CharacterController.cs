using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Script_CharacterController : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 3.0f;
    [SerializeField] private float JumpStrenght = 5.0f;
    [SerializeField] private Transform GroundChecker;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private Animator StellarAnimator;
    private float CoyoteTime = 0.1f;
    private float CoyoteTimeCounter;
    private float JumpBufferTime = 0.1f;
    private float JumpBufferCounter;

    private Rigidbody2D StellarRb;

    // Start is called before the first frame update
    void Start()
    {
        StellarRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            CoyoteTimeCounter = CoyoteTime;
        }
        else
        {
            CoyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            JumpBufferCounter = JumpBufferTime;
        }
        else
        {
            JumpBufferCounter -= Time.deltaTime;
        }

        if (CoyoteTimeCounter > 0f && JumpBufferCounter > 0f)
        {
            StellarRb.velocity = new Vector2(StellarRb.velocity.x, JumpStrenght);

            JumpBufferCounter = 0f;
        }
        if (Input.GetButtonUp("Jump") && StellarRb.velocity.y > 0f)
        {
            StellarRb.velocity = new Vector2(StellarRb.velocity.x, StellarRb.velocity.y * 0.5f);

            CoyoteTimeCounter = 0f;
        }

        Flip();

        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        Move();
    }
   
    private void Flip()
    {
        if (StellarRb.velocity.x > 0.1f)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = 1f;
            transform.localScale = localScale;
        }
        else if (StellarRb.velocity.x < -0.1f)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -1f;
            transform.localScale = localScale;
        }
    }
    private void Move()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        StellarRb.velocity = new Vector2(horizontalInput * MovementSpeed, StellarRb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(GroundChecker.position, new Vector2(0.5f, 0.2f), 0f, GroundLayer);
    }

    private void UpdateAnimator()
    {
            StellarAnimator.SetFloat("XAbsoluteSpeed", Mathf.Abs(StellarRb.velocity.x));

            StellarAnimator.SetBool("Grounded", IsGrounded());

            StellarAnimator.SetFloat("YSpeed", StellarRb.velocity.y);
    }
}
