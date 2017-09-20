using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.VR;

public class InitialScene : MonoBehaviour {


	void Awake(){
		VRSettings.enabled = false;
	}

	public void LoadTotem(){

		Initiate.FadeDefault ("Totem_StartServer");
	
	}

	public void LoadVR(){

		Initiate.FadeDefault ("VR_StartClient");
	}


}
