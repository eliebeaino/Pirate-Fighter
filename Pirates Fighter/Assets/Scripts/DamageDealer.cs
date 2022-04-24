using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100;

    public int GetDamage()                  // get damage number
    {
        return damage;
    }

    public void Hit()                       // destroy game object
    {
        Destroy(gameObject);
    }
}
