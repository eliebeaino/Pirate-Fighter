using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)                   // delete game object that collides with this object
    {
        Destroy(collision.gameObject);
    }
}
