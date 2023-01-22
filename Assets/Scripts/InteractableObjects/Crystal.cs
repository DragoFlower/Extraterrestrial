using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public GameObject drop;
    public Transform objectSpawner;
    public Transform Stellar;
    public PlayerController scriptPlayer;
    public GameObject player;   

    private void Start()
    {
        objectSpawner = transform.GetChild(0).GetComponent<Transform>();
        scriptPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Stellar = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        FindPlayer();
    }

    private void Update()
    {
        if (player == null || scriptPlayer == null || Stellar == null)
        {
            if (player != null)
            {
                FindPlayer();
            }
        }

        Drop();
    }

    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            scriptPlayer = player.GetComponent<PlayerController>();
            Stellar = player.GetComponent<Transform>();
        }
    }

    void Drop()
    {
        if (scriptPlayer != null && Stellar != null && Vector3.Distance(transform.position, Stellar.position) <= 3.0f && scriptPlayer.shootsLaser)
        {
            Instantiate(drop, objectSpawner.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
