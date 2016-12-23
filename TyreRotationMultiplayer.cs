using UnityEngine;
using System.Collections;

public class TyreRotationMultiplayer : MonoBehaviour {

    public int rawSpeed=1;
    public NetworkCar script;
    public CheckGrounded ground;
    Vector3 tyre_vel = new Vector3();

    //4Tyres array
    public GameObject[] tyresArray = new GameObject[4];

    //private bool tyre_grounded;
	// Use this for initialization
	void Start () {
        script = transform.root.GetComponent<NetworkCar>();
        tyresArray[0] = GameObject.Find("newcar/Car/Car_Outside/Tyres/Tyres_FL");
        tyresArray[1] = GameObject.Find("newcar/Car/Car_Outside/Tyres/Tyres_FR");
        tyresArray[2] = GameObject.Find("newcar/Car/Car_Outside/Tyres/Tyres_RL");
        tyresArray[3] = GameObject.Find("newcar/Car/Car_Outside/Tyres/Tyres_RR");
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Get the Velocity and check if it's grounded
        tyre_vel = script.localVel;
        //tyre_grounded = ground.grounded;


        //Apply Turn 
        foreach (GameObject tyre in tyresArray)
        {
            tyre.transform.Rotate(Vector3.right, Time.deltaTime*tyre_vel.x*rawSpeed); 
        }
	}
}
