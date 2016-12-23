using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayerID : NetworkBehaviour {

    //Sync Name Over Network
    [SyncVar]
    public string playerUniqueName;
    [SyncVar]
    public int playerNumber;
    [SyncVar]
    public bool wantsToRace;



    private NetworkInstanceId playerNetID;
    private Transform myTransform;

    public SyncListBool isReady = new SyncListBool();

    List<bool> isReadyLocal = new List<bool>();
    //Texture Variables
    NetworkCar script;
    public Texture[] multiplayerTex = new Texture[2];
    public Texture tex1;
    public Renderer rend1;

    [SerializeField]
    Behaviour[] componentsToDisable;

    //SpawnPoints
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    Transform[] worldSpawnPoints;
    private Vector3 worldSpace;

    Collider carCollider;
    Rigidbody carRB;


    //Menu GameObjects 
    public GameObject isWaitingObj;
    public GameObject isPreviewingObj;
    bool menuEnabled = false;

    //ON every player join Get his Identity and Sync it to the server
    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
        SetIdentity();
    }

    //Get this object's Net ID and sync it to the server
    [Client]
    void GetNetIdentity()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyName(MakeUniqueName(), MakeUniqueNumber());
    }

    void SetIdentity()
    {
        myTransform.name = playerUniqueName;
        if(playerNumber != 0)
        {
            //Set car color
            script.rendBody.material.mainTexture =  multiplayerTex[playerNumber-1];
            script.rendBody2.material.mainTexture =  multiplayerTex[playerNumber-1];
            script.rendBody3.material.mainTexture =  multiplayerTex[playerNumber-1];

            //Move car to spawnpoint (disable rigidbody and collider so that it doesn't jump when it spawns)
            carCollider.enabled = false;
            carRB.isKinematic = true;
            transform.position = spawnPoints[playerNumber - 1].position;
            carCollider.enabled = true;
            carRB.isKinematic = false;
        }
    }

    string MakeUniqueName()
    {
        string uniqueName = "Player" + playerNetID.ToString();
        return uniqueName;
    }

    int MakeUniqueNumber()
    {
        int uniqueNumber = int.Parse(playerNetID.ToString());
        return uniqueNumber;
         
    }

    [Command]
    void CmdTellServerIWantToRace()
    {
        wantsToRace = true;
    }

    [Command]
    void CmdTellServerMyName(string name, int number)
    {
        playerUniqueName = name;
        playerNumber = number;

    }

    void Update () {
        if(myTransform.name =="" || myTransform.name== "Car(Clone)")
        {
            SetIdentity();
        }
        if (Input.GetKeyUp("a") && isLocalPlayer)
        {
            CmdTellServerIWantToRace();
        }

        if(transform.Find("preview_camera_positions/camera_preview").GetComponent<PreviewCameraNET>().enabled == true && isLocalPlayer && Input.GetKeyUp("a"))
        {
            isWaitingObj.GetComponentInChildren<Text>().enabled = true;
            isPreviewingObj.SetActive(false);
            
        }
        if (isLocalPlayer && !menuEnabled)
        {
            GameObject.Find("PressA").GetComponentInChildren<Text>().enabled = true;
            menuEnabled = true;
            
        }
	}

    void Awake()
    {

        carCollider = gameObject.GetComponent<Collider>();
        carRB = gameObject.GetComponent<Rigidbody>();
        script = GetComponent<NetworkCar>();
        myTransform = transform;
        rend1 = transform.Find("newcar/Car/Car_Outside/carBody").GetComponent<Renderer>();
        spawnPoints[0] = GameObject.Find("Spawn1").GetComponent<Transform>();
        spawnPoints[1] = GameObject.Find("Spawn2").GetComponent<Transform>();
    }
 
    [Server]
    void InitializeListOnServer()
    {
    }

    //Disable components in array
    void disableComponents()
    {
        for  (int i =0; i<componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }


    //If not local player disable components not needed
    void Start()
    {
        InitializeListOnServer();
        if (!isLocalPlayer)
        {
            disableComponents();
        }

        //Menu INitializations
        isWaitingObj = GameObject.Find("WaitingOpponent");
        isPreviewingObj = GameObject.Find("PressA");

        if (isLocalPlayer)
        {
            isWaitingObj.GetComponentInChildren<Text>().enabled = false;
            isPreviewingObj.GetComponentInChildren<Text>().enabled = false;
        }

    }



    }
    
        


     
    


    
