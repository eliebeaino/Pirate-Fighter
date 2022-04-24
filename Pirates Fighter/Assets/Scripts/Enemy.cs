using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100f;                                                           // health value to change from unity engine manually

    [Header("Fire Mechanism")]
    [SerializeField] GameObject canonBallPrefab;
    [SerializeField] float shotCounter;                                                             // SZ for debug only
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float canonBallSpeed = 10f;
    [SerializeField] AudioClip enemyFireSound;
    [SerializeField] float fireVolume = 0.5f;

    [Header("On Death")]
    [SerializeField] GameObject explosionOnDeath;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float explosionDeathVoume = 0.5f;
    [SerializeField] int scoreValue = 150;

    private void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }
    private void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;                                                              // counter per frame
        if (shotCounter <=0f )                                                                      // when counter reaches 0, fire
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire() 
    {
        var canonBall = Instantiate(canonBallPrefab, transform.position, Quaternion.identity);       // create canon ball fired
        AudioSource.PlayClipAtPoint(enemyFireSound, Camera.main.transform.position, fireVolume);     // audio for canon ball
        canonBall.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -canonBallSpeed);            // fire direction and speed
    }

    public void OnTriggerEnter2D(Collider2D other)
    {       
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();                   // storing the game object that just bumped into it
        if (!damageDealer) { return; }                                                               // if there's no damagedealer class linked to object, return and don't proceed, this protects against null error
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)                                               // hit function that requires damageDealer paramter that was defined beforehand
    {
        health -= damageDealer.GetDamage();                                                          // health after damage
        damageDealer.Hit();                                                                          // destroy canon ball object
        if (health <= 0)                                                                             // destory object if health is 0 or less
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);                                                                          // destory the object
        GameObject deathExplosion = Instantiate(explosionOnDeath, transform.position, Quaternion.identity);        // create explosion effect
        Destroy(deathExplosion, durationOfExplosion);                                                   // destory explosion after death timer expires  - 1 sec
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, explosionDeathVoume);     // audio for explosion on death
    }
}
