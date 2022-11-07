 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Script_LavunnyController : MonoBehaviour
{
    private float LavunnySpeed = 2f;
    private Rigidbody2D LavunnyRb;
    public Transform Detector;
    public Collider2D Stellar;
    public LayerMask GroundLayer;

    // Start is called before the first frame update
    void Start()
    {
        LavunnyRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LavunnyRb.velocity = new Vector2(LavunnySpeed, 0f);

        RaycastHit2D GroundDetection = Physics2D.Raycast(Detector.position, Vector2.down, 0.5f);

        if (GroundDetection.collider == false || HittingWall())
        {
            Flip();
        }

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Stellar);
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
}
