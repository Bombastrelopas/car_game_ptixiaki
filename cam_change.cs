using UnityEngine;
using System.Collections;

public class cam_change : MonoBehaviour {

    public Camera camera1;
    public Camera camera2;
    public bool previewing;

    

	// Use this for initialization
	void Start () {
        camera1.enabled = false;
        camera2.enabled = false;

	
	}
	
	// Update is called once per frame
	void Update () {

        //if game is on(not in preview mode) change the camera from inside to outside or vice versa
        if (Input.GetKeyUp(KeyCode.Return) && !previewing)
        {
            camera1.enabled = !camera1.enabled;
            camera2.enabled = !camera2.enabled;

        }
	
	}
}
