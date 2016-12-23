using UnityEngine;
using System.Collections;

public class SpeedoMeterSingleplayer : MonoBehaviour {
    public float rotationAngle;
    Quaternion maxSpeed;
    Quaternion minSpeed;
    Quaternion startRotation, endRotation;
    NewCaz carscript;

	void Start () {

        carscript = transform.root.GetComponent<NewCaz>(); 

        maxSpeed = Quaternion.Euler(0, 0, rotationAngle);
        minSpeed = Quaternion.Euler(0, 0, -rotationAngle);

        startRotation = transform.localRotation;
        endRotation = transform.localRotation * minSpeed;
	}
	
	// Update is called once per frame
	void Update () {

        /*
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, endRotation, Time.deltaTime * 0.5f);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, startRotation, Time.deltaTime * 0.5f);
        }
        */

        
        transform.localRotation = Quaternion.Lerp(startRotation, endRotation, carscript.speedFactor);

        
        


     }
	
}
