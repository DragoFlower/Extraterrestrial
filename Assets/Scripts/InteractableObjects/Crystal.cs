using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public GameObject crystal;
    public Transform objectSpawner;
    public Transform Stellar;
    public PlayerController scriptPlayer;

    private void Start()
    {
        objectSpawner = transform.GetChild(0).GetComponent<Transform>();
    }

    private void Update()
    {
        scriptPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Stellar = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Drop();
    }

    void Drop()
    {
        if (Vector3.Distance(transform.position, Stellar.position) <= 3.0f && scriptPlayer.shootsLaser)
        {
            Instantiate(crystal, objectSpawner.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
