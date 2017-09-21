using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class ClientFunctions : MonoBehaviour {


	public void ClientConnected(){

		PlayerDevice.SetNewID (GameObject.FindObjectOfType<StartClient> ().deviceID.text);

		RegisterHandler ();

		PlayerDevice.deviceStatus = DeviceStatus.idle;

		Initiate.FadeDefault ("VR_Idle");

		VRSettings.enabled = true;
	}


	void RegisterHandler(){
		MyNetworkManager.singleton.client.RegisterHandler (MyNetworkManager.commServerToClient, ReceiveNewUser);
		MyNetworkManager.singleton.client.RegisterHandler (MyNetworkManager.commEvents, HandleEvents);
	}



	void ReceiveNewUser(NetworkMessage msg){

		if (PlayerDevice.deviceStatus == DeviceStatus.idle) {

			string[] s_msg = msg.ReadMessage<StringMessage> ().value.Split ('_');

			if (s_msg [0] == PlayerDevice.deviceID) {

				PlayerDevice.FindUserAndStartSession (s_msg [1]);


			} else {
				// not this client requested
			}

		}

	}


	// this function hanldes events sent by server to clients
	public static void HandleEvents(NetworkMessage msg) {

		string s_msg = msg.ReadMessage<StringMessage> ().value;

		if (s_msg == "ping") {

			ConfirmDeviceOnline ();

		}

		if (s_msg.Length >= 7) {


			if (s_msg.Substring (0, 7) == "warning") {

				if (PlayerDevice.deviceStatus == DeviceStatus.inSession) {

					string device = s_msg.Split ('_') [1];

					if (device == PlayerDevice.deviceID) {
				
						VRWarnings.VooEmbarcando ();

					}

				}

			}
		}


	}

	// after receiving ping, send back information on device status
	private static void ConfirmDeviceOnline(){
		
		// send confirmation back to server
		StringMessage stms = new StringMessage ();
		stms.value = PlayerDevice.deviceID + "_" + PlayerDevice.deviceStatus.ToString() ;
		MyNetworkManager.singleton.client.Send (MyNetworkManager.commClientToServer, stms);

	}


}
