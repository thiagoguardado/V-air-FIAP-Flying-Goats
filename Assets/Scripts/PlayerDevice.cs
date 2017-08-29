using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public enum DeviceStatus{
	disconnected,
	idle,
	waitingUser,
	inSession
}

public class PlayerDevice : MonoBehaviour {

	public static PlayerDevice instance = null;
	public static string deviceID = "";
	public static int currentUserID = -1;
	public static VRUser currentUser = new VRUser (-1, "null", "null", "null");
	public static DeviceStatus deviceStatus;


	// Use this for initialization
	void Awake () {

		// ensure singleton
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		deviceStatus = DeviceStatus.disconnected;

	}
		

	public static void SetNewID(string newID){

		deviceID = newID;

	}


	public static void RegisterHandler(){
		MyNetworkManager.singleton.client.RegisterHandler (MyNetworkManager.commServerToClient, ReceiveNewUser);
		MyNetworkManager.singleton.client.RegisterHandler (MyNetworkManager.commEvents, HandleEvents);
	}



	public static void ReceiveNewUser(NetworkMessage msg){
	
		if (deviceStatus == DeviceStatus.idle) {

			string[] s_msg = msg.ReadMessage<StringMessage> ().value.Split ('_');

			if (s_msg [0] == deviceID) {

				FindUserOnDB (s_msg [1]);
				SendBackConfirmationFoundUser ();
				StartSession ();

			} else {
				// not this client requested
			}

		}

	}


	// this function hanldes events sent by server to clients
	public static void HandleEvents(NetworkMessage msg) {

	}


	// find a user on db
	public static void FindUserOnDB (string userID)
	{
		//find user on database
		currentUserID = int.Parse (userID);
		VRUser user;
		instance.GetComponent<DatabaseManager> ().SetupAndRead (currentUserID, out user);
		currentUser = user;

	}

	static void SendBackConfirmationFoundUser ()
	{
		// send confirmation back to server
		StringMessage stms = new StringMessage ();
		stms.value = deviceID + "_waitingUser";
		MyNetworkManager.singleton.client.Send (MyNetworkManager.commClientToServer, stms);
	}

	public static void StartSession ()
	{
		// start session
		deviceStatus = DeviceStatus.waitingUser;
		GameObject.FindObjectOfType<IdleScreen> ().StartSession ();
	}

	public static void EndSession(){

		deviceStatus = DeviceStatus.idle;
		currentUser = new VRUser (-1, "null", "null", "null");
		currentUserID = -1;


	}

}
