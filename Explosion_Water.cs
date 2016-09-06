using UnityEngine;
using System.Collections;

public class Explosion_Water : MonoBehaviour
{
    public GameObject car;
    public AudioSource explosionAudio;
    NewCaz carscript;

   

    void Start()
    {

    }

    /* 
     * Creates a water "explosion" when the car hits a water surface. 
     * 
     * 
     * */

    IEnumerator OnTriggerEnter(Collider col)
    {

        if (col.tag == "Player")
        {

            //Create Explosion at point of impact (instantiates explosion particle and then destroys it after it plays).
            Vector3 crashPos = col.transform.position;
            explosionAudio = GameObject.Find("WaterSound").GetComponent<AudioSource>();
            explosionAudio.Play();
            Object splush = Instantiate(GameObject.Find("Splash"), crashPos, Quaternion.identity);
            Destroy(splush, 0.8f);



            //Make car the collider of the object hitting the trigger.
            car = GameObject.FindGameObjectWithTag("Player");


            //Make car the parent object.
            car = car.transform.gameObject;

            //Change the Destroyed Variable of the car
            carscript = car.GetComponent<NewCaz>();
            carscript.carDestroyed = true;
            carscript.dead_timer = 3f;


            //Reset the car speed to 0. And wait 5 seconds until it respawns somewhere.
            car.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(3.0f);

            car.transform.position = carscript.respawnPointArray[carscript.currentCheckpoint].position;
            car.transform.rotation = carscript.respawnPointArray[carscript.currentCheckpoint].rotation;



        }

    }




}
