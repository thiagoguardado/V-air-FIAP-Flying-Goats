using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationControl : MonoBehaviour {

	public static ApplicationControl instance = null;
	public static VRUser currentSelectingUser = new VRUser(0,"null","null","null","null","null");

	// Use this for initialization
	void Awake () {

		// ensure singleton
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

	}
	
	public static void FoundUserOnDatabase(VRUser _user) {
	
		// set current user
		currentSelectingUser = _user;
	
		// change to selection screen
		Initiate.FadeDefault("Totem_Confirm");

	}

	private static void SetNullUser(){
		currentSelectingUser = new VRUser(0,"null","null","null","null","null");
	}

	public static void BackToScanMenu(){

		SetNullUser ();
		Initiate.FadeDefault("Totem_Scan");

	}

}
