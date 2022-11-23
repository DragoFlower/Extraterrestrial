using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag.Equals("Player"))
       {
            Destroy(gameObject);
       }
    }
}
