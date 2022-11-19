using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    private float breakTime = 1f;
    private bool hit;
    public Script_PlayerController scriptPlayer;


    void Start()
    {
        scriptPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Script_PlayerController>();
    }

    void Update()
    {
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
