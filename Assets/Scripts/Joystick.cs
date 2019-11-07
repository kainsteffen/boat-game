using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{    
    public Transform joystickController;
    public Transform joystickHandle;

    public float inputAngle;
    public float inputDistance;
    public float minInputDistance;

    public float minSwipeDistance;
    public float minSwipeDuration;

    private bool isHolding;
    private float holdTimer;
    private Vector3 holdStartPos;

    public delegate void SwipeGesture(Vector3 direction);
    public event SwipeGesture QuickSwipe;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            joystickController.transform.position = Input.mousePosition;

        }
        else if (Input.GetMouseButton(0))
        {
            joystickHandle.position = Vector2.Lerp(joystickController.position, Input.mousePosition, 0.2f);
            Vector3 dir = (joystickHandle.position - joystickController.position).normalized;

            if (Vector3.Distance(joystickController.position, Input.mousePosition) > minInputDistance)
            {
                if(holdTimer > minSwipeDuration)
                {
                    joystickController.gameObject.SetActive(true);
                }

                inputAngle = Mathf.Atan2(-dir.y, -dir.x) * -180 / Mathf.PI - 90;
                inputDistance = Vector3.Distance(joystickController.position, Input.mousePosition);
            }
        }
        else
        {
            joystickController.gameObject.SetActive(false);
            inputDistance = 0;
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            isHolding = true;
            holdStartPos = Input.mousePosition;
            holdTimer = 0;
        }

        DetectSwipeGesture();
    }

    void DetectSwipeGesture()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
            if (holdTimer < minSwipeDuration && Vector2.Distance(holdStartPos, Input.mousePosition) > minSwipeDistance)
            {
                Vector3 direction = new Vector3(Input.mousePosition.x - holdStartPos.x, 0, Input.mousePosition.y - holdStartPos.y) / 100;
                if (QuickSwipe != null)
                {
                    QuickSwipe(direction);
                }
            }
        }

        if (isHolding)
        {
            holdTimer += Time.deltaTime;
        }
    }
}
