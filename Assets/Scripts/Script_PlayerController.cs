using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Script_PlayerController : MonoBehaviour
{
    private ScriptInput inputs;

    private void Awake()
    {
        inputs = new ScriptInput();
        inputs.Player.Jump.Enable();
        inputs.Player.Move.Enable();
        inputs.Player.Melody.Enable();
        inputs.Player.PickAxe.Enable();
        inputs.Player.PickUp.Enable();
    }

  
    public float MovementSpeed = 3.0f;
    public float JumpStrenght = 5.0f;
    private float CoyoteTime = 0.1f;
    private float CoyoteTimeCounter;
    private float JumpBufferTime = 0.1f;
    private float JumpBufferCounter;
    public Transform GroundChecker;
    public LayerMask GroundLayer;
    public Animator StellarAnimator;
    private Rigidbody2D StellarRb;
    public Collider2D MelodyRadius;
    private float MelodyTime = 1f;
    public bool Melody;

    // Start is called before the first frame update
    void Start()
    {
        StellarRb = GetComponent<Rigidbody2D>();
        MelodyRadius.enabled = false;
        Melody = false;
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

        if (inputs.Player.Jump.WasPressedThisFrame())
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
        if (inputs.Player.Jump.WasReleasedThisFrame() && StellarRb.velocity.y > 0f)
        {
            StellarRb.velocity = new Vector2(StellarRb.velocity.x, StellarRb.velocity.y * 0.5f);

            CoyoteTimeCounter = 0f;
        }

        Flip();

        UpdateAnimator();

        PlayMelody();
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
        var horizontalInput = inputs.Player.Move.ReadValue<float>();
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

    void PlayMelody()
    {
        if (inputs.Player.Melody.WasPressedThisFrame())
        {
            MelodyRadius.enabled = true;
            MelodyTime = 1f;
        }
        if (MelodyTime <= 0f && MelodyTime != float.MinValue)
        {
            MelodyRadius.enabled = false;
            MelodyTime = float.MinValue;
            Melody = false;
        }
        else if (MelodyTime > 0f)
        {
            MelodyTime -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Lifeform"))
        {
            Melody = true;
        }
    }
}
