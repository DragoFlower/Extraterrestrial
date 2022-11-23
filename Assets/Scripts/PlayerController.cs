using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    private ScriptInput inputs;
    private Rigidbody2D StellarRb;
    private Animator StellarAnimator;
    private Transform GroundChecker;
    private float coyoteTime = 0.1f;
    private float airTime;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;
    private float melodyTime = 1f;

    public LayerMask GroundLayer;
    public float speed = 3.0f;
    public float jumpStrenght = 5.0f;


    private void Awake()
    {
        inputs = new ScriptInput();
        inputs.Player.Jump.Enable();
        inputs.Player.Move.Enable();
        inputs.Player.Melody.Enable();
        inputs.Player.Laser.Enable();
        inputs.Player.Pet.Enable();
        inputs.Player.Pause.Enable();

        StellarRb = GetComponent<Rigidbody2D>();
        StellarAnimator = transform.GetChild(1).GetComponent<Animator>();
        GroundChecker = transform.GetChild(0).GetComponent<Transform>();
    }
    void Start()
    {

    }
    void Update()
    {
        if (IsGrounded())
        {
            airTime = 0f;
        }
        else
        {
            airTime += Time.deltaTime;
        }
        if (inputs.Player.Jump.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        Flip();
        UpdateAnimator();
        Melody();
        Laser();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
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
        StellarRb.velocity = new Vector2(horizontalInput * speed, StellarRb.velocity.y);
    }
    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(GroundChecker.position, new Vector2(0.3f, 0.1f), 0f, GroundLayer);
    }
    void Jump()
    {
        if (airTime < coyoteTime && jumpBufferCounter > 0f)
        {
            StellarRb.velocity = new Vector2(StellarRb.velocity.x, jumpStrenght);

            jumpBufferCounter = 0f;
        }
        if (inputs.Player.Jump.WasReleasedThisFrame() && StellarRb.velocity.y > 0f)
        {
            StellarRb.velocity = new Vector2(StellarRb.velocity.x, StellarRb.velocity.y * 0.5f);
        }
    }
    private void UpdateAnimator()
    {
        StellarAnimator.SetFloat("XAbsoluteSpeed", Mathf.Abs(StellarRb.velocity.x));

        StellarAnimator.SetBool("Grounded", IsGrounded());

        StellarAnimator.SetFloat("YSpeed", StellarRb.velocity.y);
    }
    public bool ShootsLaser()
    {
        return inputs.Player.Laser.WasPressedThisFrame();
    }

    void Laser()
    {
        if (ShootsLaser())
        {
            StellarAnimator.Play("Anim_StellarLaser");
        }
    }
    public bool PlaysMelody()
    {
        return inputs.Player.Melody.WasPressedThisFrame();
    }
    void Melody()
    {
        if (PlaysMelody())
        {
            StellarAnimator.Play("Anim_StellarMelody");
        }
    }
    public bool DoesPet()
    {
        return inputs.Player.Pet.WasPressedThisFrame();
    }
    public void Pet()
    {
        StellarAnimator.Play("Anim_StellarPet");
    }
}
