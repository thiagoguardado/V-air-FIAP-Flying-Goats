using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotemExtraFunctions : MonoBehaviour {

	public InputField device;

	public void BackToScan(){

		Initiate.FadeDefault ("Totem_Scan");

	}



	public void SendWarningEmbarqueToOne(){

		if (device.text != "") {
			SendWarningEmbarque (device.text);
		} else {
			SendWarningEmbarque (device.placeholder.GetComponent<Text>().text);
		}
	
	}


	public void SendWarningEmbarqueToAll(){
	
		for (int i = 0; i < TotemManager.devices.Length; i++) {

			SendWarningEmbarque (TotemManager.devices [i].deviceID);

		}
	
	}


	private void SendWarningEmbarque(string deviceID){

		SendInfoToVR.SendWarning (deviceID);

	}



}
