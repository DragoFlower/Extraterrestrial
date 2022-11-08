 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Script_LavunnyController : MonoBehaviour
{
    private float LavunnySpeed = 2.0f;
    private Rigidbody2D LavunnyRb;
    public Transform Detector;
    public Collider2D StellarCollider;
    public LayerMask GroundLayer;
    public Collider2D MelodyRadius;
    public Transform StellarTransform;
    public Script_PlayerController scriptPlayer;
    private bool FacingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        LavunnyRb = GetComponent<Rigidbody2D>();
        scriptPlayer = GameObject.Find("Stellar").GetComponent<Script_PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        LavunnyRb.velocity = new Vector2(LavunnySpeed, 0f);

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), StellarCollider);

        RaycastHit2D GroundDetection = Physics2D.Raycast(Detector.position, Vector2.down, 0.5f, GroundLayer);

        IsFacingPlayer();
        
        RunToMelody();
        
        if (GroundDetection.collider == false || HittingWall())
        {
            Flip();
        }   
    }

    void Flip()
    {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            LavunnySpeed *= -1f;
    }

    private bool HittingWall()
    {
        return Physics2D.OverlapCircle(Detector.position, 0.1f, GroundLayer);
    }

    void RunToMelody()
    {
        if (scriptPlayer.Melody == true && FacingPlayer == true)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, StellarTransform.position, LavunnySpeed * Time.deltaTime);
        }
        
        /*if (scriptPlayer.Melody == true && FacingPlayer == false)
        {
            Flip();

            transform.position = Vector2.MoveTowards(transform.position, StellarTransform.position, LavunnySpeed * Time.deltaTime);
        }*/
    }

    void IsFacingPlayer()
    {
        if (StellarTransform.localScale.x > transform.localScale.x && StellarTransform.position.x < transform.position.x || StellarTransform.localScale.x < transform.localScale.x && StellarTransform.position.x > transform.position.x)
        {
            FacingPlayer = true;
        }
        else
        {
            FacingPlayer = false;
        }
    }
}
