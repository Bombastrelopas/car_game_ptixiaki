using UnityEngine;
using System.Collections;

public class car : MonoBehaviour {

    public WheelCollider rightwheelF;
    public WheelCollider leftwheelF;
    public WheelCollider rightwheelB;
    public WheelCollider leftwheelB;
    public  float maxSpeed=50f;
    public Rigidbody rb;



	// Use this for initialization
	void Start () {

        rb.centerOfMass= new Vector3(rb.centerOfMass.x, -0.9f, rb.centerOfMass.z);



	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        rightwheelB.motorTorque = maxSpeed * Input.GetAxis("Vertical");
        leftwheelB.motorTorque = maxSpeed * Input.GetAxis("Vertical");
        leftwheelF.steerAngle = 10 * Input.GetAxis("Horizontal");
        rightwheelF.steerAngle = 10 * Input.GetAxis("Horizontal");


    }
}
