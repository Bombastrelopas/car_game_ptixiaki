using UnityEngine;
using System.Collections;

public class tyres_grounded : MonoBehaviour
{
    public bool front_grounded = false;

    void OnCTriggerEnter(Collider other)
    {
        
        front_grounded = true;
 
    }
    void OnTriggerStay(Collider other2)
    {

        if (other2.gameObject.tag == "Ground")
        {
            front_grounded = true;
            //Debug.Log("tyres touches Ground");
        }
        else
        {
            front_grounded = false;
        }

    }

    void OnTriggerExit(Collider other3)
    {

        //Debug.Log(other3);
        front_grounded = false;
    }




}