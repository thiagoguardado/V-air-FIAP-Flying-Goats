using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemManager : MonoBehaviour {

	public static TotemManager instance = null;

	public static DeviceInfo[] devices;


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

	void CreateDevices(){
	
		List<DeviceInfo> list = new List<DeviceInfo> ();

		string[] rows = new string[3] { "A", "B", "C" };
		string[] cols = new string[3] { "1", "2", "3" };

		for (int i = 0; i < rows.Length; i++) {
			for (int j = 0; j < cols.Length; j++) {
				list.Add(new DeviceInfo(rows[i] + cols[j],DeviceStatus.disconnected));
			}
		}
	
		devices = list.ToArray ();

	}
}


public class DeviceInfo {

	public string deviceID;
	public DeviceStatus deviceStatus;

	public DeviceInfo(string _deviceID, DeviceStatus _deviceStatus){
		deviceID = _deviceID;
		deviceStatus = _deviceStatus;
	}

}
