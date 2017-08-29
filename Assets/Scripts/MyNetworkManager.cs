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

	public Dictionary<string,int> myDevices = new Dictionary<string, int> ();


	public override void OnClientConnect (NetworkConnection conn)
	{

		PlayerDevice.SetNewID(GameObject.FindObjectOfType<StartClient>().deviceID.text);

		PlayerDevice.RegisterHandler ();

		PlayerDevice.deviceStatus = DeviceStatus.idle;

		SceneManager.LoadScene ("VR_Idle");

	}

	public override void OnStartServer ()
	{
		NetworkServer.RegisterHandler (commClientToServer, ReceiveDeviceComm);
	}


	private void ReceiveDeviceComm(NetworkMessage msg){

		string[] msgIn = msg.ReadMessage<StringMessage> ().value.Split('_');

		if (msgIn [1] == "waitingUser") {

			// do something when VR device sends back confirmation that is waiting for user

		}

	}


}
