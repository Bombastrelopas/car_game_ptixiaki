using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour {

    float distanceToTerrain = 0;
    RaycastHit hit;

	// Update is called once per frame
	void FixedUpdate () {

        //Debug.Log(LayerMask.GetMask("track"));
        Debug.DrawRay(transform.position, transform.forward * 5, Color.blue);
        if (Physics.Raycast(transform.position, transform.forward * 5 ,  out hit, 5,LayerMask.GetMask("track")))
        {

            distanceToTerrain = hit.distance;
            Debug.Log(distanceToTerrain);
            Debug.Log(hit.collider);
        }
    
	}
}
