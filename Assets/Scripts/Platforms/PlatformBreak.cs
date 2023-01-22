using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBreak : MonoBehaviour
{
    private float breakTime = 1f;
    private bool hit;
    public PlayerController scriptPlayer;
    private void Start()
    {
        scriptPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Update()
    {
        if (scriptPlayer == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                scriptPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); ;
            }
        }
        Break();
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
