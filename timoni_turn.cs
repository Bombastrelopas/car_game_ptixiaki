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
            rotato = Quaternion.Euler(0f, 0f, 30f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotato, 0.10f);

        }

        else if (Input.GetKey("right"))
        {
            rotato = Quaternion.Euler(0f, 0f, 30f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotato, 0.10f);
            
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation_initial, 0.1f);
        }



    }
}
