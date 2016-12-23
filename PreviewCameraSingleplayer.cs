using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PreviewCameraSingleplayer : MonoBehaviour   {
    public Transform[] cameraPos;
    public bool finished = false; 
    public float fraction = 0;
    public float speed = 0.2f;
    public bool previewing = true;
    //The script of camera_change to enable the camera
    cam_change camChange;
    public NewCaz mainCarScript;
    

    //Fanari states array
    public Image[] fanaria;
    
    private IEnumerator preview;


    //Fill the array with the caamera positions for the race preview
	void Start () {
        //Get the scripts corresponding to this car
        mainCarScript = transform.root.GetComponent<NewCaz>();
        camChange = transform.root.GetComponent<cam_change>();
        //Fill the array (indexes 0 and 1 will be useless)
        cameraPos = transform.parent.GetComponentsInChildren<Transform>();
        //Set first camera at position 2
        transform.position = cameraPos[2].position;
        transform.rotation = cameraPos[2].rotation;
        

        preview = RunPreviewCamera();
        StartCoroutine(preview);

        //Set the racing state to false in the maincarscript
        mainCarScript.racing = false;

        //Get Fanari GUIs
        fanaria = GameObject.Find("Fanari").GetComponentsInChildren<Image>();

        for(int i=0; i<=3; i++)
        {
            fanaria[i].enabled = false;
        }

        //Set Previewing to true so the user can't change cameras with enter on cam_change script
        camChange.previewing = true;



	}
    void Update()
    {
        //Update fraction for lerps every time it's initialized to 0
        if (fraction <= 1)
        {
            fraction += Time.deltaTime * speed;
        }

        //Stop the preview
        if (Input.GetKeyUp("a"))
        {
                StopCoroutine(preview); //Stop the routine
                camChange.camera1.enabled = true; //Enable main camera
                gameObject.GetComponent<Camera>().enabled = false; //Disable preview camerra

                //Start showing start of the race fanari
                StartCoroutine(Fanari());
        }

    }

    IEnumerator RunPreviewCamera()
    {

        while (previewing)
        {
            //Do a preview of the level
            StartCoroutine(MoveCamera(transform, cameraPos[3]));
            yield return new WaitUntil(() => fraction >= 1);
            TeleportCamera(cameraPos[4]);
            yield return new WaitForSeconds(2f);
            TeleportCamera(cameraPos[5]);
            StartCoroutine(MoveCamera(transform,cameraPos[6]));
            yield return new WaitUntil(() => fraction >= 1);
            TeleportCamera(cameraPos[7]);
            StartCoroutine(MoveCamera(transform, cameraPos[8]));
            yield return new WaitUntil(() => fraction >= 1);
            TeleportCamera(cameraPos[9]);
            yield return new WaitForSeconds(3f);
            TeleportCamera(cameraPos[2]);
        }


    }

    //Function to teleport camera from one position to another
    void TeleportCamera( Transform teleportPos)
    {
        transform.position = teleportPos.position;
        transform.rotation = teleportPos.rotation;

    }

    //Function to move camera from current position to another
    IEnumerator MoveCamera(Transform fromPos, Transform movePos)
    {
        fraction = 0;
        while(fraction < 1) 
        {
            transform.position = Vector3.Slerp(fromPos.position, movePos.position, fraction* Time.deltaTime);
            transform.rotation = Quaternion.Slerp(fromPos.rotation, movePos.rotation, fraction * Time.deltaTime);
            //fraction += Time.deltaTime * speed;
            yield return null;
        }

    }

    //Set up the starting lights and after it goes to the green start the race
    IEnumerator Fanari()
    {
        fanaria[1].enabled = true;
        yield return new WaitForSeconds(1);
        fanaria[2].enabled = true;
        fanaria[1].transform.parent.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        fanaria[3].enabled = true;
        fanaria[1].transform.parent.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        fanaria[4].enabled = true;
        camChange.previewing = false; //set the previewing variable of cam_change script to false
        mainCarScript.racing = true;  //set the car in racing mode (so it can drive and self-destruct
        fanaria[1].transform.parent.GetComponent<AudioSource>().pitch = 2.36f;
        fanaria[1].transform.parent.GetComponent<AudioSource>().volume = 3f;
        fanaria[1].transform.parent.GetComponent<AudioSource>().Play();
        
        //Make lights disappear 
        yield return new WaitForSeconds(1);
        fanaria[2].transform.root.gameObject.SetActive(false);

        //Enable music
        gameObject.transform.root.FindChild("MusicController").GetComponent<MusicBox>().enabled = true;

        //After all is done disable this script for the rest of the game
        gameObject.GetComponent<PreviewCameraSingleplayer>().enabled = false;

    }


    //Function to check if cars are ready for the race
   }
