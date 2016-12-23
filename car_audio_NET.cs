using UnityEngine;
using System.Collections;

public class car_audio_NET : MonoBehaviour {


    
    public AudioSource audio;
    public float startingpitch=0f;
    public float maxpitch = 1.2f;
    public Vector3 localVelocity;
    NewCaz carscript;


    // Use this for initialization
    void Start () {
        audio = gameObject.GetComponent<AudioSource>();
        GameObject thecar = transform.root.gameObject;
        carscript = transform.root.GetComponent<NewCaz>();
        
        



	}
	
	// Update is called once per frame
	void Update () {
        localVelocity = carscript.localVel;
        //Debug.Log(localVelocity);
        audio.pitch = startingpitch + localVelocity.z / 30f;
        if (audio.pitch> maxpitch)
        {
            audio.pitch = maxpitch;
        }
    }
}
