using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   
    public float speedEnemy = 2f;

    private Player player;

    private Animator animator;

    private AudioSource audioSource;

    [SerializeField] private GameObject laserPrefab;

    private float fireRate = 3f;

    private float canFire = -1;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if(player == null)
        {
            Debug.Log("The Player is NULL");
        }

        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.Log("the Animator is NULL");
        }

        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            Debug.LogError("The AudioSource is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        if(Time.time > canFire)
        {
            fireRate = Random.Range(3f, 7f);
            canFire = Time.time + fireRate;
            GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i< lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * speedEnemy * Time.deltaTime);

        if (transform.position.y <= -5)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 8f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
            animator.SetTrigger("OnEnemyDeath");
            speedEnemy = 0;
            audioSource.Play(); 
            Destroy(gameObject,0.75f);
           
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if(player != null)
            {
                player.AddScore(10);
            }
            animator.SetTrigger("OnEnemyDeath");
            speedEnemy = 0;
            audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject,0.75f);
           
        }
    }
}
