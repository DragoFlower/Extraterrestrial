using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavunnyController : MonoBehaviour
{
    public PlayerController scriptPlayer;
    public LayerMask GroundLayer;    
    public GameObject egg;
    public Transform Stellar;
    public bool stop;

    private Collider2D StellarCollider;
    private Transform objectSpawner;
    private Transform detector;
    private Rigidbody2D rb;
    private Animator animator;
    private float speed = 2.0f;
    private float isListeningTimer;
    private float eggCount = 1f;

    void Start()
    {
        detector = transform.GetChild(0).GetComponent<Transform>();
        objectSpawner = transform.GetChild(1).GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isListeningTimer = float.MinValue;
        stop = false;
    }

    void Update()
    {
        StellarCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        scriptPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Stellar = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), StellarCollider);

        RaycastHit2D GroundDetection = Physics2D.Raycast(detector.position, Vector2.down, 0.5f, GroundLayer);

        if (GroundDetection.collider == false || HittingWall())
        {
            Flip();
        }

        if (stop)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
        else
        {
            rb.velocity = new Vector2(speed * transform.localScale.x, 0f);
        }

        MelodyListening();

        Stop();

        Pet();

        UpdateAnimator();
    }
    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    private bool HittingWall()
    {
        return Physics2D.OverlapCircle(detector.position, 0.1f, GroundLayer);
    }

    private void Stop()
    {
        if (Vector3.Distance(transform.position, Stellar.position) <= 3.0f && scriptPlayer.playsMelody)
        {
            stop = true;
            isListeningTimer = 5f;
            if (!FacingPlayer())
                Flip();
        }
    }
    void MelodyListening()
    {
        if (isListeningTimer <= 0f && stop)
        {
            stop = false;
        }
        if (isListeningTimer > 0f && stop)
        {
            isListeningTimer -= Time.deltaTime;
        }
    }

    public void Pet()
    {
        if (scriptPlayer.DoesPet() && BothFacing() && stop && Vector3.Distance(transform.position, Stellar.position) <= 1.0f)
        {
            scriptPlayer.Pet();
            animator.Play("Anim_LavunnyPet");

            if (isListeningTimer <= 1f)
                isListeningTimer += 1f;

            if (eggCount > 0f)
            {               
                Instantiate(egg, objectSpawner.position, Quaternion.identity);
                eggCount = 0f;
            }
        }
    }

    private bool FacingPlayer()
    {
        return ((transform.position.x > Stellar.transform.position.x && transform.localScale.x < Stellar.transform.localScale.x) ||
            (transform.position.x > Stellar.transform.position.x && transform.localScale.x == -1f && Stellar.transform.localScale.x == -1f) ||
            (transform.position.x < Stellar.transform.position.x && transform.localScale.x > Stellar.transform.localScale.x) ||
            (transform.position.x < Stellar.transform.position.x && transform.localScale.x == 1f && Stellar.transform.localScale.x == 1f));
    }

    private bool BothFacing()
    {
        return ((transform.position.x > Stellar.transform.position.x && transform.localScale.x < Stellar.transform.localScale.x) || (transform.position.x < Stellar.transform.position.x && transform.localScale.x > Stellar.transform.localScale.x));
    }

    void UpdateAnimator()
    {
        if (stop)
            animator.SetBool("Stop", true);

        if (!stop)
            animator.SetBool("Stop", false);
    }
}
