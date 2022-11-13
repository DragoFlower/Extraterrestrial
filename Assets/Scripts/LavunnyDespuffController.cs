using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Windows;

public class LavunnyDespuffController : MonoBehaviour
{
    private float speed = 2.0f;
    private Rigidbody2D rb;
    private Animator animator;
    private float isListeningTimer;
    
    public Transform Detector;
    public LayerMask GroundLayer;
    public Collider2D StellarCollider;
    public Collider2D MelodyRadius;
    public Script_PlayerController scriptPlayer;
    public Collider2D PetRadius;
    public bool canBePet;

    private void Start()
    {
        isListeningTimer = float.MinValue;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        scriptPlayer = GameObject.Find("Stellar").GetComponent<Script_PlayerController>();
        PetRadius.enabled = false;
    }

    private void Update()
    {
        if (isListeningTimer == float.MinValue)
        {
            rb.velocity = new Vector2(speed, 0f);
        }

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), StellarCollider);

        RaycastHit2D GroundDetection = Physics2D.Raycast(Detector.position, Vector2.down, 0.5f, GroundLayer);

        if (GroundDetection.collider == false || HittingWall())
        {
            Flip();
        }

        MelodyListening();

        Pet();
    }
    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        speed *= -1f;
    }
    private bool HittingWall()
    {
        return Physics2D.OverlapCircle(Detector.position, 0.1f, GroundLayer);
    }

    void MelodyListening()
    {
        if (scriptPlayer.playMelody && isListeningTimer == float.MinValue)
        {
            rb.velocity = new Vector2(0f, 0f);
            isListeningTimer = 5f;
            PetRadius.enabled = true;
        }
        if (isListeningTimer <= 0f && isListeningTimer != float.MinValue)
        {
            rb.velocity = new Vector2(speed * transform.localScale.x, 0f);
            isListeningTimer = float.MinValue;
            PetRadius.enabled = false;
            canBePet = false;
        } 
        if (isListeningTimer > 0f)
        {
            isListeningTimer -= Time.deltaTime;
        }
    }

    private void Pet()
    {
        if (scriptPlayer.beingPet == true)
        {
            PetRadius.enabled = false;
            scriptPlayer.beingPet = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && scriptPlayer.beingPet != true)
        {
            canBePet = true;
        }
    }
}
