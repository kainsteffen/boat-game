using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Joystick joystick;
    public ParticleSystem dashParticle;
    public ParticleSystem waterBubbleParticle;
    public GameObject dashImpactParticle;
    public GameObject cannonBall;
    public List<GameObject> cannons;

    public bool autoShoot;
    public float shootCooldown;
    private float shootTimer;

    public bool physicalMovement;
    public float physicalMoveForce;
    public float physicalSpeedLimit;

    public bool isometricMovement;

    public float maxSpeed;
    public float cannonShootForce;
    public float dashDamage;
    private bool isDashing = false;

    private float buttonPressTimer;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        joystick = GetComponent<Joystick>();
        shootTimer = shootCooldown;
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        joystick.QuickSwipe += Dash;
    }

    // Update is called once per frameWhy 
    void Update()
    {

        if(isometricMovement)
        {
            print(Quaternion.Euler(0, joystick.inputAngle, 0).eulerAngles.y);
            transform.rotation = Quaternion.Euler(0, LimitToIsometricAngle(Quaternion.Euler(0, joystick.inputAngle, 0).eulerAngles.y), 0);
        } else
        {
            transform.rotation = Quaternion.Euler(0, joystick.inputAngle, 0);
        }
        
        
        if(joystick.inputDistance > 0)
        {
            if(physicalMovement)
            {
                rb.AddForce(transform.forward * physicalMoveForce);
                if(rb.velocity.magnitude > physicalSpeedLimit)
                {
                    rb.velocity = rb.velocity.normalized * physicalSpeedLimit;
                }
            } else
            {
                transform.Translate(Vector3.forward * Mathf.Min(joystick.inputDistance, maxSpeed) * Time.deltaTime);
            }
            
            waterBubbleParticle.Play();
        } else
        {
            waterBubbleParticle.Stop();
        }

        if(Input.GetMouseButtonDown(0))
        {
            buttonPressTimer = 0.3f;
        }

        if(Input.GetMouseButton(0))
        {
            buttonPressTimer -= Time.deltaTime;
        }

        if(autoShoot)
        {
            shootTimer -= Time.deltaTime;
            if(shootTimer < 0)
            {
                ShootCannons();
                shootTimer = shootCooldown;
            }
        } else
        {
            if (Input.GetMouseButtonUp(0) && !(buttonPressTimer < 0) && !isDashing)
            {
                ShootCannons();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if(collider.tag == "Enemy" && isDashing)
        {
            collider.GetComponent<Enemy>().TakeDamage(dashDamage);
            Instantiate(dashImpactParticle, Vector3.Lerp( collision.GetContact(0).point, Camera.main.transform.position, 0.5f), dashImpactParticle.transform.rotation);
            StartCoroutine(FreezeFrame());
        }
    }

    IEnumerator FreezeFrame()
    {
        float originalTimescale = Time.timeScale;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = originalTimescale;
    }

    void Dash(Vector3 direction)
    {
        isDashing = true;
        StartCoroutine(EndDashState());
        transform.rotation = Quaternion.LookRotation(direction);
        GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
        dashParticle.Play();
    }

    IEnumerator EndDashState()
    {
        yield return new WaitForSeconds(0.5f);
        isDashing = false;
    }

    void ShootCannons()
    {
        foreach (GameObject cannon in cannons)
        {
            GameObject newCannonBall = Instantiate(cannonBall, cannon.transform.position, cannon.transform.rotation);
            newCannonBall.GetComponent<Rigidbody>().AddForce(cannon.transform.forward * cannonShootForce);
        }
    }

    float LimitToIsometricAngle(float angle)
    {
        float newAngle = angle;
        float segmentSize = 45 / 2;
        if (angle >= 0)
        {
            if (angle < 16 * segmentSize)
            {
                newAngle = 360;
            }

            if (angle < 15 * segmentSize)
            {
                newAngle = 315;
            }

            if (angle < 14 * segmentSize)
            {
                newAngle = 315;
            }

            if (angle < 13 * segmentSize)
            {
                newAngle = 270;
            }

            if (angle < 12 * segmentSize)
            {
                newAngle = 270;
            }

            if (angle < 11 * segmentSize)
            {
                newAngle = 225;
            }

            if (angle < 10 * segmentSize)
            {
                newAngle = 225;
            }

            if (angle < 9 * segmentSize)
            {
                newAngle = 180;
            }

            if (angle < 8 * segmentSize)
            {
                newAngle = 180;
            }

            if (angle < 7 * segmentSize)
            {
                newAngle = 135;
            }

            if (angle < 6 * segmentSize)
            {
                newAngle = 135;
            }

            if (angle < 5 * segmentSize)
            {
                newAngle = 90;
            }

            if (angle < 4 * segmentSize)
            {
                newAngle = 90;
            }

            if (angle < 3 * segmentSize)
            {
                newAngle = 45;
            }

            if (angle < 2 * segmentSize)
            {
                newAngle = 45;
            }

            if (angle < 1 * segmentSize)
            {
                newAngle = 0;
            }

        }

        return newAngle;
    }
}
