using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartServer : MonoBehaviour {

	public InputField address;
	public InputField port;
	public Button start;

	void Awake(){
	
		address.placeholder.GetComponent<Text> ().text = Network.player.ipAddress;
	
	}

	public void Init ()
	{
		if (address.text == "") {
			address.text = address.placeholder.GetComponent<Text> ().text;
		}

		if (port.text == "") {
			port.text = port.placeholder.GetComponent<Text> ().text;
		}

		MyNetworkManager.singleton.networkAddress = address.text;
		MyNetworkManager.singleton.networkPort = int.Parse(port.text);
		MyNetworkManager.singleton.StartServer ();

		Initiate.FadeDefault ("Totem_Scan");

	}
}
