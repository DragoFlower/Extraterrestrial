using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrystalDropPickup : MonoBehaviour
{
    public GameObject GameController;
    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag.Equals("Player"))
       {
            CustomEvent.Trigger(GameController, "AddStoneScore");
            Destroy(gameObject);
       }
    }
}
