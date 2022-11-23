using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystals : MonoBehaviour
{
    public GameObject crystal;
    public Transform objectSpawner;
    public Transform Stellar;
    public PlayerController scriptPlayer;

    private void Start()
    {
        objectSpawner = transform.GetChild(0).GetComponent<Transform>();
        scriptPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Stellar = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Drop();
    }

    void Drop()
    {
        if (Vector3.Distance(transform.position, Stellar.position) <= 1.0f && scriptPlayer.ShootsLaser())
        {
            Instantiate(crystal, objectSpawner.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
