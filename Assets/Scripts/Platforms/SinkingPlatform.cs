using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour
{
    private bool hit;
    private float speed = 0.1f;
    public PlayerController scriptPlayer;
    public Transform origin;

    void Update()
    {
        scriptPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Sink();
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
        hit = false;
    }

    void Sink()
    {
        if (hit)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(origin.position.x, origin.position.y - 0.5f), speed * Time.deltaTime);
        }
        if (!hit)
        {
            transform.position = Vector3.MoveTowards(transform.position, origin.position, speed * Time.deltaTime);
        }
    }
}

