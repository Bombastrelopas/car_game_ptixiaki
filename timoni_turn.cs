using UnityEngine;
using System.Collections;

public class timoni_turn : MonoBehaviour {

    public Quaternion rotation_initial;
    public Quaternion rotato;



	// Use this for initialization
	void Start () {
        rotation_initial = transform.localRotation;
        
	}
	
	// Update is called once per frame
	void Update () {

        


        if (Input.GetKey("left"))
        {
            rotato = Quaternion.Euler(-1.7974f, 77.5429f, 58.0161f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotato, 0.15f);

        }

        else if (Input.GetKey("right"))
        {
            rotato = Quaternion.Euler(10.2286f, 80.7875f, -48.4994f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotato, 0.15f);
            
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation_initial, 0.1f);
        }



    }
}
