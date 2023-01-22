using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBreak : MonoBehaviour
{
    private float breakTime = 1f;
    private bool hit;
    public PlayerController scriptPlayer;
    public GameObject player;
    private void Start()
    {
        FindPlayer();
    }
    void Update()
    {
        Break();

        if (scriptPlayer == null || player == null)
        {
                FindPlayer();
        }  
    }
    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            scriptPlayer = player.GetComponent<PlayerController>();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (scriptPlayer.OnBreakingPlatform())
        {
            hit = true;
        }
    }

    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (hit)
            Destroy(gameObject);
    }

    void Break()
    {
        if (hit)
        {
            if (breakTime > 0f)
            {
                breakTime -= Time.deltaTime;
            }
            else if (breakTime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
