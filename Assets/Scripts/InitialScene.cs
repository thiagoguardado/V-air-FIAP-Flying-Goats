using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.VR;

public class InitialScene : MonoBehaviour {


	void Awake(){
		VRSettings.enabled = false;;
	}

	public void LoadTotem(){

		SceneManager.LoadScene ("Totem_StartServer");
	
	}

	public void LoadVR(){


		SceneManager.LoadScene("VR_StartClient");
	}


}
