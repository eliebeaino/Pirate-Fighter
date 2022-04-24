using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] float health = 200f;

    [Header("Projectile")]
    [SerializeField] GameObject canonBallPrefab;
    [SerializeField] float canonBallSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.2f;

    [Header("On Death")]
    [SerializeField] GameObject explosionOnDeath;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float explosionDeathVoume = 0.5f;

    Coroutine fireCoroutine;

    // camera variables for setting up boundries
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()                                                                                 // fire input for player
    {
        if (Input.GetButtonDown("Fire1"))
        {
            fireCoroutine = StartCoroutine(FiringRoutine());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(fireCoroutine);
        }
    } 
    private IEnumerator FiringRoutine()                                                                  // repeat fire while holding button
    {
        while (true)
        {
            GameObject canonBallFired = Instantiate(canonBallPrefab, transform.position, Quaternion.identity);
            canonBallFired.GetComponent<Rigidbody2D>().velocity = new Vector2(0, canonBallSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    } 

    private void Move()                                                                                  // move input for the player
    {
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        float newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2 (newXPos, newYPos);
    } 
    private void SetUpMoveBoundries()                                                                     // camera boundries for movement
    {
        Camera gameCamera;
        gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3 (0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
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
        Destroy(gameObject);
        GameObject deathExplosion = Instantiate(explosionOnDeath, transform.position, Quaternion.identity);        // create explosion effect
        Destroy(deathExplosion, durationOfExplosion);                                                              // destory explosion after death timer expires  - 1 sec
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, explosionDeathVoume);              // audio for explosion on death
        FindObjectOfType<Level>().LoadGameOver();                                                                  // game over screen
       }

    public int GetHealth()
    {
        int healthInt = (int)health;
        return healthInt;
    }
}