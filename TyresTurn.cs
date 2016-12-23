using UnityEngine;
using System.Collections;

public class TyresTurn : MonoBehaviour {

    public Quaternion targetRotation;
    public Quaternion rotationInitial;
	// Use this for initialization
	void Start () {
	
        rotationInitial = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("right"))
        {
            targetRotation = Quaternion.Euler(0f, 15f, 0f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, 0.15f);

        }
        else if (Input.GetKey("left"))
        {
            targetRotation = Quaternion.Euler(0f, -15f, 0f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, 0.15f);

        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotationInitial, 0.1f);
        }



	}
} 