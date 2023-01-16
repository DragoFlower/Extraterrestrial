using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public GameObject plant;
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
        if (Vector3.Distance(transform.position, Stellar.position) <= 2.5f && scriptPlayer.shootsLaser)
        {
            Instantiate(plant, objectSpawner.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
