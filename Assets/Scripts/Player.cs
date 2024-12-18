using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject tripleShotPrefab;

    public float speed = 3.5f;
    [SerializeField] private float speedMultiplier = 2f;

    [SerializeField] private float fireRate = 0.5f;
    private float canFire = -1f;

    [SerializeField] private int lives = 3;

    private SpawnManager spawnManager;

    [SerializeField] private bool isTripleShotActive = false;
    [SerializeField] private bool isSpeedBoostActive = false;

    [SerializeField] private bool isShieldsActive = false;

    [SerializeField] private GameObject shieldVisualizer;

    [SerializeField] private GameObject rightEngine;

    [SerializeField] private GameObject leftEngine;

    [SerializeField] private int score ;

    private UI_Manager ui_Manager;

    [SerializeField] private AudioClip laserSoundClip;

    private AudioSource audioSource;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if(spawnManager == null)
        {
            Debug.LogError("the Spawn manager is NULL .");
        }

        ui_Manager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if(ui_Manager == null)
        {
            Debug.LogError("the UI_Manager is NULL .");
        }

        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            Debug.LogError("The AudioSource is NULL .");
        }
        else
        {
            audioSource.clip = laserSoundClip;
        }
    }


    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
        {
            FireLaser();

        }
    }

    void FireLaser()
    {
        canFire = Time.time + fireRate;

        if (isTripleShotActive == true)
        {
            Instantiate(tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(laserPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        }

        audioSource.Play();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        
        
       transform.Translate(direction * speed * Time.deltaTime);
        




        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        //  transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,-3.8f, 0) , 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

   

    public void Damage()
    {
        if(isShieldsActive == true)
        {
            isShieldsActive = false;
            shieldVisualizer.SetActive(false);
            return;
        }

        lives --;

        if(lives == 2)
        {
            leftEngine.SetActive(true);
        }
        else if(lives == 1)
        {
            rightEngine.SetActive(true);
        }

        ui_Manager.UpdateLives(lives);


        if(lives <1)
        {
            spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        isTripleShotActive = false;

    } 

    public void SpeedBoostActive()
    {
        isSpeedBoostActive = true;
        speed = speed * speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        isSpeedBoostActive = false;
        speed = speed / speedMultiplier;
    }

    public void ShieldsActive()
    {
        isShieldsActive = true;
        shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        score += points;
        ui_Manager.UpdateScore(score);
    }
}

