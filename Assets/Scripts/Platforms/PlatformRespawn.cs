using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformRespawn : MonoBehaviour
{
    private float respawnTime = 2f;
    public GameObject platform;
    public GameObject platformPrefab;


    void Start()
    {
        platform = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (platform == null)
        {
            respawnTime -= Time.deltaTime;
            if (respawnTime <= 0f)
            {
                Instantiate(platformPrefab, transform.position, Quaternion.identity, gameObject.transform);
                platform = transform.GetChild(0).gameObject;
            }
        }

        if (platform != null)
            respawnTime = 2f;
    }
}
