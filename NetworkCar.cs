using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class NetworkCar : NetworkBehaviour
{

    //Basic Settings speed/mass etc
    public float speed = 0f;
    public float MaxSpeed = 600000f;
    public Vector3 MaxVelocity = new Vector3(30, 0, 0);
    public float MaxBreak = -800000f;
    public float Acceleration = 1000000f;
    public float Deceleration = 800000f;
    public float turnAngle = 70;
    public float lerpVal = 0.15f;
    public Rigidbody rb;
    public float ExtraCarWeight = 500;
    public float mass;

    //Texture variables
    public Texture alternativeEmission;
    public Texture normalEmission;
    public Renderer rend;
    public Renderer rend2;
    public Renderer rendBody;
    public Renderer rendBody2;
    public Renderer rendBody3;

    //Other Booleans 
    public bool isboosted=false;
    public float boost_timer = 7;

    [SyncVar]
    public float dead_timer = 0;
    public bool isGroundedBefore;

    [SyncVar (hook = "OnCarDestroyed")]
    public bool carDestroyed=false;

    public bool gamePaused;
    public bool racing = false;  //If the car hasn't finished


    //Check buttons pressed (control variables)
    private bool turnsLeft;
    private bool turnsRight;
    [SerializeField]   private bool handBreakDeactivated = false;
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
    public Vector3 localVelb;

    //Other Stuff
    public GameObject wheels;

    //Trail_Emitters STuff
    public GameObject trailEmitter_R;
    public GameObject trailEmitter_L;
    public GameObject trailEmitter_Pref;

    public Transform  trailEmitterTrans;
    public GameObject trailEmitterInstantiated;
    public bool skidding;
    public bool alreadySkidding;

    public GameObject smokeEmitter;

    //GUI variables
    public Texture2D metroMeter;
    public Texture2D needle;
    public float speedFactor;
    public float needleAngle;

    
    //Raycasts
    float distanceToGround;
    RaycastHit hit;
    RaycastHit hit_ground;
    RaycastHit tyres_hit_ground;
    public float raycastLength = 5f;
    public float raycastFrontLength = 0.7f;

    public float something = 0.7f;

    //Checkpoints variables and arrays
    public int currentCheckpoint;
    public GameObject[] checkPointArray;
    public Transform[] respawnPointArray = new Transform[7];
    public int checknum = 0;

    public int currentLap;
    public bool[] fairRace = new bool[7];

    //UI Variables
    public Text timeTimer;
    public float lapTime;
    public float bestTime;
    public GameObject pauseMenu;

    //BlurScripts
    public Blur[] blurScripts;

    //Network booleans
    bool RoutineRunning;
    public bool raceFinished = false;
    public bool raceWon = false;



    // Get the Rigidbody
    void Start()
    {

        //Creates checkpoints array and orders it by name
        checkPointArray = GameObject.FindGameObjectsWithTag("CheckPoint").OrderBy( go => go.name ).ToArray();

        foreach (GameObject cp in checkPointArray)
        {
            respawnPointArray[checknum] = cp.transform;
            checknum++;
        } 


        //Other initializations
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
        currentLap = 1;
        gamePaused = false;

        //Skidmarks and Smoke Initialization
        trailEmitter_R = gameObject.transform.Find("Skidmarks_Emitters/skidR").gameObject;
        trailEmitter_L = gameObject.transform.Find("Skidmarks_Emitters/skidL").gameObject;


        trailEmitter_Pref = GameObject.Find("Skidmarks_Emitters");
        trailEmitterTrans = trailEmitter_Pref.transform;

        skidding = false;
        alreadySkidding = false;

        smokeEmitter = gameObject.transform.FindChild("Smoke_Emitters").gameObject;

        //Set emitters to inactive
        trailEmitter_Pref.SetActive(false);
    
        //Set respawnpoint as start
        currentCheckpoint = 0;


        //Get text component of GUI 
        timeTimer = GameObject.Find("TimeText").GetComponent<Text>();
        bestTime = 0;

        //Get Blur Scripts which will be enabled when game is paused
        blurScripts = GetComponentsInChildren<Blur>();

        //Set the normal emission texture as the current onee
        rend = GameObject.Find("rearRightLight").GetComponent<Renderer>();
        rend2 = GameObject.Find("rearLeftLight").GetComponent<Renderer>();
        rendBody = transform.Find("newcar/Car/Car_Outside/carBody").GetComponent<Renderer>();
        normalEmission = rend.material.GetTexture("_EmissionMap");


        /*
        foreach(Transform child in GameObject.Find("CheckPoints").transform)
        {
            Debug.Log(child);
            Debug.Log(checkpoints.Length);
            Debug.Log(checkPointArray);
        } 
        */


        //Other stuff for tests delete after.
        //rb.centerOfMass = new Vector3(rb.centerOfMass.x, rb.centerOfMass.y, rb.centerOfMass.z+1);
        //transform.position = new Vector3(438.05f, 1.09f, 99.79f);
        racing = true;
    }


    void OnCarDestroyed(bool destroyed)
    {
        if (destroyed && !RoutineRunning)
        {
            StartCoroutine(CarSelfDestruct(transform.position));
        }
    }



    //Update Function
    void Update()
    {

        if (raceFinished && isLocalPlayer)
        {
            if (raceWon)
            {
                Debug.Log("You won!");
            }
            else
            {
                Debug.Log("You lost!");
            }

        }
        if (!isLocalPlayer || !racing)
        {
            return;
        }


        //GUI updates
        lapTime += Time.deltaTime;
        timeTimer.text = lapTime.ToString("F3");


        //Check if the car is moving if not then set it to nomoving(so that it won't turn) 
        if (transform.InverseTransformDirection(rb.velocity).z > 0.01f || transform.InverseTransformDirection(rb.velocity).z < -0.01f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
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
        if (Input.GetKeyUp("space") && isGrounded)
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
        if (Input.GetKey("right") && isGrounded && isMoving && TyresGrounded)
        {
            turnsRight = true;
        }
        else
        {
            turnsRight = false;

        }
        if (Input.GetKey("left") && isGrounded && isMoving && TyresGrounded)
        {
            turnsLeft = true;
        }
        else
        {
            turnsLeft = false;
        }

        //Destroy Car if destruction button pressed
        if (Input.GetKeyUp("d") && !gamePaused && !carDestroyed)
        {
            CmdtellServerCarDestroyed(true);
        }

        //Pause the game if (S) is pressed.
        if (Input.GetKeyUp("s"))
        {
            if (Time.timeScale == 1.0F)
            {
                foreach (Blur blur in blurScripts)
                {
                    blur.enabled = true;
                }

                pauseMenu.SetActive(true);
                gamePaused = true;
                Time.timeScale = 0.0F;
            }
            else
            {
                foreach (Blur blur in blurScripts)
                {
                    blur.enabled = false;
                }
                pauseMenu.SetActive(false);
                gamePaused = false;
                Time.timeScale = 1.0F;
            }
        }

        //Exit game if user presses ESC and game is paused
        if (Input.GetKeyUp(KeyCode.Escape) && gamePaused)
        {
            pauseMenu.SetActive(false);
            gamePaused = false;
            Time.timeScale = 1.0F;
            SceneManager.LoadScene(0);

        }
    

        
        /* SKIDMARKS LOGIC */
        SkidmarksLogic();

    }

    /* SKIDMARKS LOGIC */
    void SkidmarksLogic()
    {

        //If car is on the ground and user is pressing space create a new instance of skidmarks
        if (Input.GetKey(KeyCode.Space) && !skidding && isGrounded) 
        {
            skidding = true;
            //Enable the skidmarks original prefab instantiate it and then set it inactive again till it's needed again.
            trailEmitter_Pref.SetActive(true);
            trailEmitterInstantiated = Instantiate(trailEmitter_Pref, new Vector3(0f, 0.3f, -2.01f) , trailEmitter_Pref.transform.localRotation) as GameObject;
            trailEmitterInstantiated.transform.SetParent(transform,false);
            trailEmitter_Pref.SetActive(false);
            
            
        } 

        if (!isGrounded && (trailEmitterInstantiated != null) )
        {
            Destroy(trailEmitterInstantiated, 20f);
            trailEmitterInstantiated.transform.parent = null;
            skidding = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (trailEmitterInstantiated != null)
            {
                Destroy(trailEmitterInstantiated, 20f);
                trailEmitterInstantiated.transform.parent = null;

            }
           skidding = false;
        }

        //Smoke Emitters enable/disablposition
        if (Input.GetKeyDown(KeyCode.E))
        {
            smokeEmitter.SetActive(!smokeEmitter.activeSelf);
        }
    }

   
 
    void FixedUpdate()
    {
        //Make car-meshes invisible if it dies.
        if (dead_timer <= 0)
        {   //Make visible
            if (isLocalPlayer)
            {
                CmdtellServerCarDestroyed(false);
            }
            rb.isKinematic = false;
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.enabled = true;
            }
            foreach (SkinnedMeshRenderer skinnedrendere in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                skinnedrendere.enabled = true;
            }
            gameObject.transform.root.Find("Smoke_Emitters/Smoke_L").gameObject.SetActive(true);
            gameObject.transform.root.Find("Smoke_Emitters/Smoke_R").gameObject.SetActive(true);
        }
        else
        {   //Make invisible
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                rb.isKinematic = true;
                renderer.enabled = false;
            }
            foreach (SkinnedMeshRenderer skinnedrendere in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                skinnedrendere.enabled = false;
            }
            gameObject.transform.root.Find("Smoke_Emitters/Smoke_L").gameObject.SetActive(false);
            gameObject.transform.root.Find("Smoke_Emitters/Smoke_R").gameObject.SetActive(false);
        }

        //Reduce Timers by 1 every second. If 0 keep it at 0.
        if (boost_timer > 0)
        {
            boost_timer = boost_timer - Time.deltaTime;
        }

        if (dead_timer > 0)
        {
            dead_timer = dead_timer - Time.deltaTime;
        }

        if (!isLocalPlayer)
        {
            return;
        }


        //Take the variable from tyresgrounded script and put it in this script
        TyresGrounded = GetComponentInChildren<tyrecheck>().TyresGrounded; 

        //Collider distance to ground
        distanceToGround = gameObject.GetComponent<Collider>().bounds.extents.y;

        //Some raycast to help with falling
        Ray LandingRay = new Ray(GetComponent<Collider>().bounds.center, Vector3.down);
        Debug.DrawRay(LandingRay.origin, Vector3.down * raycastLength, Color.blue);

        if (Physics.Raycast(LandingRay, out hit, raycastLength))
        {
            if (hit.collider.tag == "Ground" && hit.distance > 4f)
            {

                localVelb = localVel;
                localVelb.z = localVelb.z - (2 * localVelb.y);
            }
        }



        //Use above raycast to check if grounded
        if (Physics.Raycast(LandingRay, out hit_ground, raycastLength))
        {
            if (hit_ground.collider.tag == "Ground" && hit_ground.distance < distanceToGround + 0.1f)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }











        //Changes the velocity of the car so that it doesn't lose velocity when it jumps and reaches the ground
        if (!isGroundedBefore && isGrounded)
        {
            //Debug.Log("VelocityB");
            //Debug.Log(localVelb);
            
            rb.velocity = transform.TransformDirection(localVelb);
            //Debug.Log(rb.velocity);
        }
        

        //Check to see if front of the parat is touching the ground(so the car won't go forward or jump when it falls on its "butt"(it won't get any forces until it falls smoothly).
        //TyresGrounded = gameObject.GetComponentInChildren<tyres_grounded>().front_grounded;

        //Add a bit of mass to the car when it flys so it falls down faster and more normal.
        if (!isGrounded)
        {
            localVelb = localVel;
            isGroundedBefore = false;
            rb.mass = mass + ExtraCarWeight;
        }
        else
        {
            
            isGroundedBefore = true;
            rb.mass = mass;
        }

        

         
        //Find the local Velocity of the car
        localVel = transform.InverseTransformDirection(rb.velocity);



        //Add force relative to speed
        if (isGrounded && !carDestroyed)
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
        if (boost_timer<=0)
        {

            //Check if velocity has exceeded a limit and if yes keep it at the maximum
            if ((transform.InverseTransformDirection(rb.velocity).z > MaxVelocity.x || transform.InverseTransformDirection(rb.velocity).z < -MaxVelocity.x) && !boost)
            {
                //Debug.Log("changed vel");

                //Keep max velocity when going forward
                if (transform.InverseTransformDirection(rb.velocity).z > 0)
                {
                    localVel = transform.InverseTransformDirection(rb.velocity);
                    localVel.z = MaxVelocity.x;
                    rb.velocity = transform.TransformDirection(localVel);
                }
                //Keep -max velocity when going backward
                else
                {
                    localVel = transform.InverseTransformDirection(rb.velocity);
                    localVel.z = -MaxVelocity.x;
                    rb.velocity = transform.TransformDirection(localVel);
                }

            }
        }


        //Custom downward force to do gravity checks depending on level. Will be used with trigger colliders to make the car jump correctly on ramps of different lengths.
        
        rb.AddForce(0, -customGrav, 0);
        



    
    }

    IEnumerator CarSelfDestruct(Vector3 carPos)
    {
        RoutineRunning = true;
        gameObject.GetComponent<AudioSource>().Play();
        Object boom=Instantiate(GameObject.Find("Explosion"), carPos, Quaternion.identity);
        CmdtellServerCarDestroyed(true);
        CmdsetDeadTimer(3f);
        

        yield return new WaitForSeconds(3.0f);
        CmdtellServerCarDestroyed(false);
        RoutineRunning = false;

        transform.position = respawnPointArray[currentCheckpoint].position;
        transform.rotation = respawnPointArray[currentCheckpoint].rotation;
    }

    [Command]
    void CmdsetDeadTimer(float deadTimer)
    {
        dead_timer = deadTimer;
    }
    [Command]
    void CmdtellServerCarDestroyed(bool destroyedState)
    {
        carDestroyed = destroyedState;
    }

    /*
    //GUI Design
    void OnGUI()
    {
      
        //Draw metrometer
        GUI.DrawTexture(new Rect(Screen.width-309, Screen.height-283, 400,400), metroMeter);
       

        
        //Rotate needle
        GUIUtility.RotateAroundPivot(needleAngle, new Vector2(Screen.width- 110.2f, Screen.height- 82.1f));
        GUI.DrawTexture(new Rect(Screen.width - 309, Screen.height - 283, 400, 400), needle);
        

        //Calculate needle angle
        speedFactor = localVel.z / 30;
        needleAngle = Mathf.Lerp(0, 270, speedFactor);


    }
    */




}