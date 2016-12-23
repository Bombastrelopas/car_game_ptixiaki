using UnityEngine;
using System.Collections;

public class TyreRotationSingleplayer : MonoBehaviour {

    public int rawSpeed=1;
    public NewCaz newCazScript;
    public CheckGrounded ground;
    Vector3 tyre_vel = new Vector3();
    //private bool tyre_grounded;
	// Use this for initialization
	void Start () {
        newCazScript = transform.root.GetComponent<NewCaz>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Get the Velocity and check if it's grounded
        tyre_vel = newCazScript.localVel;
        //tyre_grounded = ground.grounded;


        //Apply Turn 
        transform.Rotate(Vector3.right, Time.deltaTime*tyre_vel.x*rawSpeed); 
	}
}
