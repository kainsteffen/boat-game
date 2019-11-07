using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject onDeathParticle;
    public float lifeTime;
    public float damage;

    // Start is called before the first frame update

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terrain")
        {
            Die();
        }

        if (gameObject.tag == "Player" && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Die();
        }

        if (gameObject.tag == "Enemy" && collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    void Die()
    {
        Instantiate(onDeathParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
