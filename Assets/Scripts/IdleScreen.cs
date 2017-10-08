using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleScreen : MonoBehaviour {

	public void StartSession(){

		PlayerDevice.deviceStatus = DeviceStatus.inSession;

		Initiate.FadeDefault ("VR_Intro");

	}


}
