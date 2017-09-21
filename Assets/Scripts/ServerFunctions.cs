using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ServerFunctions : MonoBehaviour {


	public void ServerStarted(){
		
		NetworkServer.RegisterHandler (MyNetworkManager.commClientToServer, ReceiveDeviceComm);

	}

	private void ReceiveDeviceComm(NetworkMessage msg){

		string msgIn = msg.ReadMessage<StringMessage> ().value;

		Debug.Log ("Device online: " + msgIn);

		string[] msgIn_ = msgIn.Split('_');


		// do something when VR device sends back confirmation that waiting for user
		for (int i = 0; i < TotemManager.devices.Length; i++) {

			if (TotemManager.devices [i].deviceID == msgIn_ [0]) {
				if (msgIn_[1] == DeviceStatus.idle.ToString()) {
					TotemManager.devices [i].deviceStatus = DeviceStatus.idle;
				}
			}

		}



	}

}
