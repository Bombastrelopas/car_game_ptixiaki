using UnityEngine;
using System.Collections;

public class tyrecheck : MonoBehaviour {

    RaycastHit tyreshit;
    public float distanceToGround;
    public float raycastFrontLength;
    public bool TyresGrounded;


	// Use this for initialization

    /* Uses a raycast in the front trigger of the car to check if the front part is grounded
    If it isn't it doesn't allow the car to turn and helps the car not to jump when it falls 
    on its butt
    */

	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {




        distanceToGround= gameObject.GetComponent<Collider>().bounds.extents.y;
 

        Ray FrontRay = new Ray(transform.position , Vector3.down);
        Debug.DrawRay(FrontRay.origin, Vector3.down *distanceToGround);

        if (Physics.Raycast(FrontRay, out tyreshit, distanceToGround*5))
        {
            if (tyreshit.collider.tag == "Ground" && tyreshit.distance < distanceToGround + 0.1f)
            {
                
                TyresGrounded = true;
            }
            else
            {
                
                TyresGrounded = false;
            }
        }


    } 
}
