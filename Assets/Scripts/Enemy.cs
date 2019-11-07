using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public ParticleSystem waterBubbleParticle;
    public GameObject cannonBall;
    public List<GameObject> cannons;
    public float maxHealth;
    private float currentHealth;
    public float speed;
    public float shootRange;
    public float shootCooldown;
    public float cannonShootForce;

    private float shootTimer;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            Die();
        } else
        {
            if (Vector3.Distance(transform.position, target.position) > shootRange)
            {
                shootTimer = Random.Range(0, shootCooldown);
                waterBubbleParticle.Play();
                transform.LookAt(target);
                transform.position += transform.forward * speed;
            } else
            {
                shootTimer -= Time.deltaTime;
                waterBubbleParticle.Stop();
                transform.right = (target.position - transform.position).normalized;
                if(shootTimer < 0)
                {
                    ShootCannons();
                }
            }
        }
    }

    void ShootCannons()
    {
        foreach (GameObject cannon in cannons)
        {
            GameObject newCannonBall = Instantiate(cannonBall, cannon.transform.position, cannon.transform.rotation);
            newCannonBall.GetComponent<Rigidbody>().AddForce(cannon.transform.forward * cannonShootForce);
        }
        shootTimer = shootCooldown;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    void Die()
    {
        waterBubbleParticle.Play();
        Physics.IgnoreLayerCollision(9, 10);
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        Destroy(gameObject, 10);
    }
}
