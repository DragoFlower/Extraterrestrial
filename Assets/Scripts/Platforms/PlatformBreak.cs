using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBreak : MonoBehaviour
{
    private float breakTime = 1f;
    private bool hit;
    public PlayerController scriptPlayer;

    void Update()
    {
        scriptPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Break();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (scriptPlayer.IsGrounded())
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
