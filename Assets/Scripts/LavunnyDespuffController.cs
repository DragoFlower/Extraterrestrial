using UnityEngine;

public class LavunnyDespuffController : MonoBehaviour
{
    private float speed = 2.0f;
    private Rigidbody2D rb;
    private Animator animator;
    private float isListeningTimer;
    
    public Transform Detector;
    public LayerMask GroundLayer;
    public Collider2D StellarCollider;
    public bool stop = false;
    public GameObject egg;
    public Transform objectSpawner;

    private void Start()
    {
        Detector = transform.GetChild(0).GetComponent<Transform>();
        objectSpawner = transform.GetChild(1).GetComponent<Transform>();
        isListeningTimer = float.MinValue;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StellarCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), StellarCollider);

        RaycastHit2D GroundDetection = Physics2D.Raycast(Detector.position, Vector2.down, 0.5f, GroundLayer);

        if (GroundDetection.collider == false || HittingWall())
        {
            Flip();
        }

        if (stop)
        {
            rb.velocity = new Vector2(0f, 0f);
        } else { 
            rb.velocity = new Vector2(speed, 0f);
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
        if (isListeningTimer <= 0f && stop)
        {
            Run();
        }
        if (isListeningTimer > 0f && stop)
        {
            isListeningTimer -= Time.deltaTime;
        }
    }

    public void Pet()
    {
        isListeningTimer = 2f;
        Debug.Log("Pet"+this.name);
        Instantiate(egg, objectSpawner.position, Quaternion.identity);
    }

    public void Stop()
    {
       if (!stop)
        {
            stop = true;
            isListeningTimer = 5f;
       }
   }

    public void Run()
    {
        if(stop)
        {
            stop = false;
        }
    }
}
