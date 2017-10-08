using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.VR;

public class InitialScene : MonoBehaviour {


	void Awake(){
		VRSettings.enabled = false;

		VRControl vrcontrol = GameObject.FindObjectOfType<VRControl> ();
		if (vrcontrol != null) {
			Destroy (vrcontrol.gameObject);
		}

	}

	public void LoadTotem(){

		Initiate.FadeDefault ("Totem_StartServer");
	
	}

	public void LoadVR(){

		Initiate.FadeDefault ("VR_StartClient");
	}


}
