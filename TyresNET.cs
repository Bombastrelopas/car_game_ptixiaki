using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TyresNET : NetworkBehaviour {

    [SyncVar]
    public Quaternion syncTyreRot;

    [SyncVar]
    public Quaternion syncTyre2Rot;


    [SerializeField]
    Transform tyresRightTransform;

    [SerializeField]
    Transform tyresLeftTransform;

    [SerializeField]
    float lerpRate = 15;

    
	void FixedUpdate () {
        TransmitPosition();
        LerpPosition();

	
	}

    void LerpPosition()
    {
        if(!isLocalPlayer)
        {
            tyresRightTransform.rotation = Quaternion.Lerp(tyresRightTransform.rotation, syncTyreRot, Time.deltaTime * lerpRate);
            tyresLeftTransform.rotation = Quaternion.Lerp(tyresLeftTransform.rotation, syncTyre2Rot, Time.deltaTime * lerpRate);
        }
    }




    [Command]
    void CmdProvidePositionToServer(Quaternion rot, Quaternion rot2)
    {
       syncTyreRot = rot;
       syncTyre2Rot = rot2;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer)
        {
            CmdProvidePositionToServer(tyresRightTransform.rotation, tyresLeftTransform.rotation);
        }
    }
}
