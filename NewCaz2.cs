using UnityEngine;
using System.Collections;

public class NewCaz2 : MonoBehaviour
{

    //Basic Settings
    public float speed = 0f;
    public float MaxSpeed = 600000f;
    public int MaxVelocity = 30;
    public float MaxBreak = -800000f;
    public float Acceleration = 1000000f;
    public float Deceleration = 800000f;
    public float turnAngle = 70;
    public float lerpVal = 0.15f;
    public Rigidbody rb;
    public float ExtraCarWeight = 500;

    //Other Booleans (for boost checks)
    public bool isboosted=false;
    public float boost_timer = 7;

    //Check buttons pressed
    private bool turnsLeft;
    private bool turnsRight;
    [SerializeField]   private bool handBreakDeactivated = false;
    public CheckGrounded script;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool boost=false;
    public bool TyresGrounded;

    public float customGrav = 100f;

    //Variables for lerp
    private float t;
    private bool lerpZ;
    


    //Local Velocity will be used in some parts to help override unwanted velocities/forces.
    public Vector3 localVel;


    // Get the Rigidbody
    void Start()
    {
        script = GetComponent<CheckGrounded>();
        rb = GetComponent<Rigidbody>();
        
    }


    void Update()
    {
        //Take the variable isGrounded from the other script which checks the collision with the ground
        isGrounded = script.grounded;
        //Debug.Log(localVel);


        //Check if the car is moving 
        if (rb.velocity == Vector3.zero)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }



        //Check handbreak
        if (Input.GetKey("space"))
        {
            handBreakDeactivated = false;
        }
        else
        {
            handBreakDeactivated = true;

        }

      

        //If user releases space the car stops drifting and lerps to forward velocity.
        if (Input.GetKeyUp("space"))
        {
            lerpVal = 0.05f; //Lerp value smaller so the transition is slower since it's using handbreak
            localVel = transform.InverseTransformDirection(rb.velocity);
            t = 0;
            lerpZ = true;
        }



        //Acceleration-Deceleration movement which will be used to give force to the car
        if ((Input.GetKey("down")) && (speed > MaxBreak))
        {
            speed = speed - Acceleration * Time.deltaTime;
        }
        else if ((Input.GetKey("up")) && (speed < MaxSpeed))
        {
            speed = speed + Acceleration * Time.deltaTime;
        }
        else
        {
            if (speed > Deceleration * Time.deltaTime)
            {
                speed = speed - Deceleration * Time.deltaTime;
            }
            else if (speed < -Deceleration * Time.deltaTime)
            {
                speed = speed + Deceleration * Time.deltaTime;
            }
            else
            {
                speed = 0;
            }

        }
        if (Input.GetKey("right")  && isMoving)
        {
            turnsRight = true;
        }
        else
        {
            turnsRight = false;

        }
        if (Input.GetKey("left")  && isMoving)
        {
            turnsLeft = true;
        }
        else
        {
            turnsLeft = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Check to see if front of the parat is touching the ground(so the car won't go forward or jump when it falls on its "butt"(it won't get any forces until it falls smoothly).
        //TyresGrounded = gameObject.GetComponentInChildren<tyres_grounded>().front_grounded;

        //Add a bit of mass to the car when it flys so it falls down faster and more normal.
        /*
        if (!isGrounded)
        {
            rb.mass = 1100 + ExtraCarWeight;

        }
        else
        {
            rb.mass = 1100;
        }
        */
        //Debug.Log(rb.velocity);             

        localVel = transform.InverseTransformDirection(rb.velocity);


        //Add force relative to speed

        if (true)
        {
            rb.AddRelativeForce(Vector3.forward * speed * Time.deltaTime);
        }
        //If user presses right button without handbreak
        if (turnsRight && handBreakDeactivated)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, turnAngle, 0) * Time.deltaTime));


            localVel = transform.InverseTransformDirection(rb.velocity);
            t = 0;
            lerpVal = 0.08f;
            lerpZ = true; 

        }

        

        //If User turns right with handbreak the car "slides" and turns faster +20 degrees.
        else if (turnsRight && !handBreakDeactivated)
        {
            //Debug.Log("Turning without handbreak");
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, turnAngle+20, 0) * Time.deltaTime));
        }

        //Same for left
        if (turnsLeft && handBreakDeactivated)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, -turnAngle, 0) * Time.deltaTime));


            localVel = transform.InverseTransformDirection(rb.velocity);
            t = 0;
            lerpVal = 0.08f;
            lerpZ = true;

        }
        else if (turnsLeft && !handBreakDeactivated)
        {
            //Debug.Log("Turning without handbreak");
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, -turnAngle - 20, 0) * Time.deltaTime));
        }

        //Smoothly change the car direction towards the place it faces with Lerp
        if (lerpZ)
        {
            //Debug.Log("lerpZ Happened yo yo yo");
            t += lerpVal;
            rb.velocity = transform.TransformDirection(new Vector3(Mathf.Lerp(localVel.x, 0, t), 0, localVel.z));
            if (t >= 1)
            {
                lerpZ = false;
                handBreakDeactivated = false;
                t = 0;
            }
        }


        //Check if car has some form of power boosting it. If yes have no speed limit. Else have a speed limit.
        if (true)
        {

            Debug.Log("MaxVel:");
            Debug.Log(MaxVelocity);
            Debug.Log("MaaxvelocINversedirection");
            Debug.Log(transform.InverseTransformDirection(rb.velocity).z);

            //Check if velocity has exceeded a limit and if yes keep it at the maximum
            if ((transform.InverseTransformDirection(rb.velocity).z > MaxVelocity || transform.InverseTransformDirection(rb.velocity).z < -MaxVelocity))
            {
                Debug.Log("changed vel11");

                //Keep max velocity when going forward
                if (transform.InverseTransformDirection(rb.velocity).z > 0)
                {
                    
                    localVel = transform.InverseTransformDirection(rb.velocity);
                    localVel.z = MaxVelocity;
                    rb.velocity = transform.TransformDirection(localVel);
                }
                //Keep -max velocity when going backward
                else
                {
                    localVel = transform.InverseTransformDirection(rb.velocity);
                    localVel.z = -MaxVelocity;
                    rb.velocity = transform.TransformDirection(localVel);
                }

            }
        }


        //Custom downward force to do gravity checks depending on level. Will be used with trigger colliders to make the car jump correctly on ramps of different lengths.
        /*
        rb.AddForce(0, -customGrav, 0);
        */

        //Reduce Boost Timer by 1 every second. If 0 keep it at 0.
        if (boost_timer > 0)
        {
            boost_timer = boost_timer - Time.deltaTime;
        }


    
    }
}