using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 3f;

    [SerializeField] GameObject explosionPrefab;

    private SpawnManager spawnManager;
    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            spawnManager.StartSpawning();
            Destroy(this.gameObject ,0.2f);
        }
    }
}
