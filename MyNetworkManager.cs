using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkBehaviour {

    public GameObject carPrefab;

    NetworkClient myClient;

    public void SetupClient()
    {
        ClientScene.RegisterPrefab(carPrefab);

        myClient = new NetworkClient();

        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect("localhost", 7777);
    }

    void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Client connected");
    }

    [Command]
    void CmdSpawn()
    {
        Debug.Log("CMDSPAWN");
        var go = (GameObject)Instantiate(carPrefab, transform.position, Quaternion.identity);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }

    public void Start()
    {
        Debug.Log("HEY");
        SetupClient();
        CmdSpawn();
        Debug.Log("HEY");

    }



}
