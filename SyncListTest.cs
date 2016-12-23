using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class SyncListTest : NetworkBehaviour {
    [SyncVar]
    public SyncListInt someList = new SyncListInt();
	void Start () {
	
	}
	
    [Command]
    void CmdAddNumberToList(int number)
    {
          someList.Add(number);
//        RpcUpdateC();
    }
/*
    [Server]
    void updateClients()
    {
        RpcUpdateC();
    }
    */
    [ClientRpc]
    void RpcUpdateC()
    {
        someList.Add(4);
    }

	void Update () {
        if (Input.GetKeyUp("p") && isLocalPlayer)
        {
            CmdAddNumberToList(3);
        }

        Debug.Log(string.Format("{0} === {1}", someList.Count, gameObject.name.ToString()));
	}

}
