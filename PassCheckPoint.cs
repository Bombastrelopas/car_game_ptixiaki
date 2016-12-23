﻿using UnityEngine;
using System.Collections;

public class PassCheckPointMultiplayer : MonoBehaviour {

    public GameObject collidingCar;
    public int CheckPointZoneNumber;
    NetworkCar script;

	// Use this for initialization
	void Start () {
        
	
	}


    void OnTriggerEnter (Collider col)
    {
        if (col.tag == "Player")
        {
            /*
            Debug.Log(col.gameObject);
            Debug.Log(CheckPointZoneNumber);
            */
            script = col.gameObject.GetComponent<NetworkCar>();
            script.currentCheckpoint = CheckPointZoneNumber;
            //Set true the array index of the car checkpoints to true
            script.fairRace[CheckPointZoneNumber] = true;


        }
    }
   



	
}
