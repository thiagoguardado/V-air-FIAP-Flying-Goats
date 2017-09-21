using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class SendInfoToVR : MonoBehaviour {


	public static void FindDevice(string deviceID, string userID){
	
		StringMessage stms = new StringMessage ();
		stms.value = deviceID + "_" + userID;

		NetworkServer.SendToAll (MyNetworkManager.commServerToClient, stms);

	}

	public static void PingAllDevices(){

		StringMessage stms = new StringMessage ();
		stms.value = "ping";

		NetworkServer.SendToAll (MyNetworkManager.commEvents, stms);

	}


	public static void SendWarning(string deviceID) {

		StringMessage stms = new StringMessage ();
		stms.value = deviceID + "_warning";

		NetworkServer.SendToAll (MyNetworkManager.commEvents, stms);



	}

}
