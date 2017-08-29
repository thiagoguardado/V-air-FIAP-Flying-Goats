using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.VR;

public class InitialScene : MonoBehaviour {

	public NetworkManager netManager;

	void Awake(){
		VRSettings.enabled = false;;
	}

	public void LoadTotem(){

		NetworkManager.singleton.StartServer ();
		SceneManager.LoadScene ("Totem_StartServer");
	
	}

	public void LoadVR(){


		SceneManager.LoadScene("VR_StartClient");
	}


}
