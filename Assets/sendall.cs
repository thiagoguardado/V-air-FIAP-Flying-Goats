using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class sendall : MonoBehaviour {



	public void SendDeviceToAll(){
	
		StringMessage stms = new StringMessage ();
		stms.value = "A1";

		NetworkServer.SendToAll (MyNetworkManager.commServerToClient, stms);
	
	}

}
