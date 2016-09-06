using UnityEngine;
using System.Collections;

public class CheckGrounded : MonoBehaviour
{
    public bool grounded = false;
    
    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col);
        grounded = true;
    }
    void OnCollisionStay(Collision col2)
    {
        Debug.Log("OnCOllisionSyau happened");
        if (col2.gameObject.tag == "Ground")
        {
            grounded = true;
            //Debug.Log("Car touches Ground");
        }
        else
        {
            grounded = false;
        }

    }

    void OnCollisionExit(Collision col3)
    {
        Debug.Log("OnCOllisionExit happened");
        grounded = false;
    }

}

