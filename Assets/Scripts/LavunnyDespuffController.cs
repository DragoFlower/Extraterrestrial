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

    private void Start()
    {
        isListeningTimer = float.MinValue;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        scriptPlayer = GameObject.Find("Stellar").GetComponent<Script_PlayerController>();
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
        if (scriptPlayer.Melody && isListeningTimer == float.MinValue)
        {
            rb.velocity = new Vector2(0f, 0f);
            isListeningTimer = 5f;
        }
        if (isListeningTimer <= 0f && isListeningTimer != float.MinValue)
        {
            rb.velocity = new Vector2(speed * transform.localScale.x, 0f);
            isListeningTimer = float.MinValue;
        } 
        if (isListeningTimer > 0f)
        {
            isListeningTimer -= Time.deltaTime;
        }
    }
}
