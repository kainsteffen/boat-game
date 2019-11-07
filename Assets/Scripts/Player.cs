using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float floatDepth; 
    public float cannonShootForce; 
    public GameObject cannonBall;
    public List<GameObject> cannons;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            ShootCannons();
        }
    }

    void ShootCannons()
    {
        foreach(GameObject cannon in cannons)
        {
            GameObject newCannonBall = Instantiate(cannonBall, cannon.transform.position, cannon.transform.rotation);
            newCannonBall.GetComponent<Rigidbody>().AddForce(cannon.transform.forward * cannonShootForce);
        }
    }
}
