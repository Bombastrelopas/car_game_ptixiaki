using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class cam_change_NET : NetworkBehaviour {

    public Camera camera1;
    public Camera camera2;
    public bool previewing;

    

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            return;
        }
        camera1 = transform.FindChild("camera_behind").GetComponent<Camera>(); 
        camera2 = transform.FindChild("camera_inside").GetComponent<Camera>(); 
        camera1.enabled = true;
        camera2.enabled = false;
        
 } 
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
        {
            return;
        }
        //if game is on(not in preview mode) change the camera from inside to outside or vice versa
        if (Input.GetKeyUp(KeyCode.Return))
        {
            camera1.enabled = !camera1.enabled;
            camera2.enabled = !camera2.enabled;

        }
	
	}
}
