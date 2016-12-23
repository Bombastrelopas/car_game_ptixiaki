using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetPlz : NetworkBehaviour {

    //Variables which will be synced to the server
    [SyncVar (hook = "SyncPositionValues")]
    private Vector3 syncPos;

    [SyncVar]
    private Quaternion syncCarRotation;

    [SyncVar]
    public bool isBreaking;

    /// <HELLO>
    /// SOME SYNCVAR VARIABLES FOR CURRENT LAP
    /// </HELLO>
    public int userID = 0;


    [SerializeField]
    Transform myTransform;
    private float lerpRate = 15;
    private float normalLerpRate = 15;
    private float fasterLerpRate = 25;

    //CarTexture Variables
    public Texture[] multiplayerTex;
    public Renderer rendLeftLight;
    public Renderer rendRightLight;

    //CarLightsTextures
    public Texture lightsOn;
    public Texture lightsOff;

    //NetEfficiency
    private List<Vector3> syncPosList = new List<Vector3>();
    [SerializeField]
    private bool useHistoricalLerping = false;
    private float closeEnough = 0.2f;
   

    void Start()
    {

        lerpRate = normalLerpRate;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        TransmitPosition();
        LerpPosition();
        TransmitBreakState();
        SyncLight();
	
	}

    void SyncLight()
    {
        if (isBreaking)
        {
            rendRightLight.material.SetTexture("_EmissionMap", lightsOn);
            rendLeftLight.material.SetTexture("_EmissionMap", lightsOn);
        }
        else
        {
            rendRightLight.material.SetTexture("_EmissionMap", lightsOff);
            rendLeftLight.material.SetTexture("_EmissionMap", lightsOff);
        }

    }


    void LerpPosition()
    {
        if(!isLocalPlayer)
        {
            if (useHistoricalLerping)
            {
                HistoricalLerping();
            }
            else
            {
                OrdinaryLerping();
            }

        }
    }

    [Client]
    void SyncPositionValues(Vector3 latestPos)
    {
        syncPos = latestPos;
        syncPosList.Add(syncPos);

    }

    void OrdinaryLerping()
    {
        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncCarRotation, Time.deltaTime * lerpRate);
    }

    void HistoricalLerping()
    {
        if (syncPosList.Count > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * lerpRate);
            if (Vector3.Distance(myTransform.position, syncPosList[0]) < closeEnough)
            {
                syncPosList.RemoveAt(0);
            }

            if (syncPosList.Count > 10)
            {
                lerpRate = fasterLerpRate;
            }
            else
            {
                lerpRate = fasterLerpRate;
            }
        }

    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer)
        {
            CmdProvidePositionToServer(myTransform.position, myTransform.rotation);
        }
    }
    [ClientCallback]
    void TransmitBreakState()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                CmdProvideBreakStatusToServer(true);
            }
            else
            {
                CmdProvideBreakStatusToServer(false);
            } 
        }
    }

    //Commands that transmit the state and position to the server
    [Command]
    void CmdProvideBreakStatusToServer(bool state)
    {
        isBreaking = state;

    }
    [Command]
    void CmdProvidePositionToServer(Vector3 pos, Quaternion rot)
    {
        syncPos = pos;
        syncCarRotation = rot;
    }

   

}
