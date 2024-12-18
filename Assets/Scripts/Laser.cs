using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speedLaser = 8f;

    private bool isEnemyLaser = false;  
    void Update()
    {
        if(isEnemyLaser == false)
        {
            LaserMoveUp();
        }
        else
        {
            LaserMoveDown();
        }
    }

    void LaserMoveUp()
    {
        transform.Translate(Vector3.up * speedLaser * Time.deltaTime);


        if (transform.position.y > 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    void LaserMoveDown()
    {
        transform.Translate(Vector3.down * speedLaser * Time.deltaTime);


        if (transform.position.y < -8)
        {
            if (transform.parent != null)   
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
            
        }
    }
}
