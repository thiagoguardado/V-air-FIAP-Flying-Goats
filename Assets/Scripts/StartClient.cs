using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class StartClient : MonoBehaviour {

	public InputField address;
	public InputField port;
	public InputField deviceID;
	public int demoUserID = 1;

	public void Init ()
	{
		if (address.text == "") {
			address.text = address.placeholder.GetComponent<Text> ().text;
		}

		if (port.text == "") {
			port.text = port.placeholder.GetComponent<Text> ().text;
		}
			
		if (deviceID.text == "") {
			deviceID.text = deviceID.placeholder.GetComponent<Text> ().text;
		}

		MyNetworkManager.singleton.networkAddress = address.text;
		MyNetworkManager.singleton.networkPort = int.Parse(port.text);
		MyNetworkManager.singleton.StartClient ();

	}


	public void StartDemo(){

		VRSettings.enabled = true;

		PlayerDevice.deviceStatus = DeviceStatus.inSession;

		PlayerDevice.SetNewID(deviceID.text);
		PlayerDevice.FindUserOnDB (demoUserID.ToString());

		Initiate.FadeDefault ("VR_Intro");

	}
		
}
