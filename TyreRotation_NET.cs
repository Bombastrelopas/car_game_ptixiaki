using UnityEngine;
using System.Collections;

public class TyreRotation_NET : MonoBehaviour {

    public int rawSpeed=1;
    public NetworkCar script;
    public CheckGrounded ground;
    Vector3 tyre_vel = new Vector3();
    //private bool tyre_grounded;
	// Use this for initialization
	void Start () {
        script = transform.root.GetComponent<NetworkCar>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Get the Velocity and check if it's grounded
        tyre_vel = script.localVel;


        //Apply Turn 
        transform.Rotate(Vector3.right, Time.deltaTime*tyre_vel.x*rawSpeed); 
	}
}
