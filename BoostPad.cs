using UnityEngine;
using System.Collections;

public class BoostPad : MonoBehaviour {
    //Make timer start on Newcaz so that the car can reach higher limits and get a slight boost. 
    //Script notes: See notes on middle.

    public int boost_strength = 4;
    public float timer;
    NewCaz script;
    GameObject thecar;
    Rigidbody carrb;


    // Update is called once per frame


    void Start()
    {
        thecar = GameObject.Find("Car 1");  //<-------------NEEDS FIX NOT TO GET IT LIKE THIS BUT BY THE ONE IT COLLIDES WITH
        script = thecar.GetComponent<NewCaz>();
    }

    void Update () {
        //Debug.Log(car);
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col);
        if (col.tag == "Player")
        {
            Debug.Log("collided");
            script.boost_timer = 7f;
            carrb=thecar.GetComponent<Rigidbody>();
            carrb.AddForce(boost_strength*Vector3.right*100000);
            Debug.Log(carrb);
        }
        

    }


}
