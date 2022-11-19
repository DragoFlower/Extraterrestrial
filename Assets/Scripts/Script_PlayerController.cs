using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Script_PlayerController : MonoBehaviour
{
    private ScriptInput inputs;

    private void Awake()
    {
        inputs = new ScriptInput();
        inputs.Player.Jump.Enable();
        inputs.Player.Move.Enable();
        inputs.Player.Melody.Enable();
        inputs.Player.Laser.Enable();
        inputs.Player.Pet.Enable();
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
    public bool playMelody;
    public List<GameObject> Lavunnys = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StellarRb = GetComponent<Rigidbody2D>();
        playMelody = false;
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

        Pet();

        Lavunny();

        Laser();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(GroundChecker.position, new Vector2(0.3f, 0.1f));
    }

    private void Lavunny()
    {
        // Durchsuche die Liste der Lavunnys im Collider
        for (int i = Lavunnys.Count - 1; i >= 0; i--)
        {
            GameObject lavunny = Lavunnys[i];

            // In welcher Richtung und Entfernung ist der Lavunny
            float toLavunny = transform.position.x - lavunny.transform.position.x;

            // Ist der Lavunny noch in Melodie-Reichweite?
            if (toLavunny < 3.5 && toLavunny > -3.5)
            {
                // Spielt die Musik, wird der Lavunny angehalten
                if(playMelody)
                {
                    LavunnyDespuffController lavunnyScript = lavunny.GetComponent<LavunnyDespuffController>();
                    lavunnyScript.Stop();
                }
            }
            else
            {
                // Wenn der Lavunny nicht mehr in Melodie-Reichweite ist, wird er aus der Liste gelöscht
                Lavunnys.Remove(lavunny);
            }
        }
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

    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(GroundChecker.position, new Vector2(0.3f, 0.1f), 0f, GroundLayer);
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
            MelodyTime = 1f;
            StellarAnimator.Play("Anim_StellarMelody");
            playMelody = true;
        }
        if (MelodyTime <= 0f && playMelody)
        {
            playMelody = false;
        }
        else if (MelodyTime > 0f && playMelody)
        {
            MelodyTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Lifeform"))
        {
            if(!Lavunnys.Contains(collision.gameObject))
            {
                Lavunnys.Add(collision.gameObject);                
            }
        }
    }
    private void Pet()
    {
        if (inputs.Player.Pet.WasPressedThisFrame())
        {
            bool pet = false;
            for (int i = Lavunnys.Count - 1; i >= 0; i--)
            {
                GameObject lavunny = Lavunnys[i];

                float toLavunny = transform.position.x - lavunny.transform.position.x;
                // Ist der Lavunny in Streichelreichweite (zwischen 0.1 und 0.5) und in der richtigen Richtung, kann gestreichelt werden
                if (((toLavunny < 0.5 && toLavunny > 0.1) || (toLavunny > -0.5 && toLavunny < -0.1)) && Math.Sign(toLavunny) != Math.Sign(transform.localScale.x))
                {
                    LavunnyDespuffController lavunnyScript = lavunny.GetComponent<LavunnyDespuffController>();
                    lavunnyScript.Pet();
                    pet = true;
                }
            }
            if(pet)
            {
                StellarAnimator.Play("Anim_StellarPet");
            }
        }
    }
    public bool laser()
    {
        return (inputs.Player.Laser.WasPressedThisFrame());
    }

    void Laser()
    {
        if (laser())
        {
            StellarAnimator.Play("Anim_StellarLaser");
        }
    }
}
