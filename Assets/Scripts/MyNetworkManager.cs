using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.VR;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager {

	public static short commServerToClient = 100;
	public static short commClientToServer = 120;
	public static short commEvents = 130;

	private ClientFunctions clientF;
	private ServerFunctions serverF;


	public override void OnClientConnect (NetworkConnection conn)
	{
		GetComponent<ClientFunctions> ().ClientConnected ();
	}

	public override void OnStartServer ()
	{
		GetComponent<ServerFunctions> ().ServerStarted ();
	}





}
