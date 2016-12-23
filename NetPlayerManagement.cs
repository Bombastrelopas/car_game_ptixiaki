using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetPlayerManagement : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (NetworkServer.connections.Count > 0)
        {
            Debug.Log(gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId);
            Debug.Log(NetworkServer.connections[0]);
            if (NetworkServer.connections.Count > 1)
            {
                Debug.Log(NetworkServer.connections[1]);
            }
        }

        
	}
}
