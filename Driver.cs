using UnityEngine;
using System.Collections;

public class Driver : MonoBehaviour {

    int isTurningHash = Animator.StringToHash("IsTurning");
    int isTurningRightHash = Animator.StringToHash("IsTurningRight");
    AnimatorStateInfo currentState;

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {

        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool(isTurningHash, true);
        }
        else
        {
            anim.SetBool(isTurningHash, false);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool(isTurningRightHash, true);
        }
        else
        {
            anim.SetBool(isTurningRightHash, false);
        }
	
	}
}
