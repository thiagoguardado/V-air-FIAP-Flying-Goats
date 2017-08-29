using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System;
using System.Data;
using Mono.Data.SqliteClient;

public class QRCodeReader : MonoBehaviour {

	//camera
	private WebCamTexture camTexture;
	private Rect panelRect;
	private Rect cameraRect;
	private bool isActive = true;

	private DatabaseManager dbManager;

	void Awake(){

		// get reference
		dbManager = GetComponent<DatabaseManager> ();
		panelRect = transform.GetComponent<RectTransform> ().rect;

		//start camera application
		StartCamera ();


	}


	private void StartCamera ()
	{
		// calculate camera position
		Vector2 cornerPos = new Vector2 (transform.position.x - panelRect.width / 2, Screen.height - (transform.position.y + panelRect.height / 2));
		cameraRect = new Rect (cornerPos.x, cornerPos.y, panelRect.width, panelRect.height);

		WebCamDevice[] devices = WebCamTexture.devices;

		bool found = false;

		for (int i = 0; i < devices.Length; i++) {
			if (devices [i].isFrontFacing) {
				camTexture = new WebCamTexture (devices [i].name);
				found = true;
				break;
			}
		}

		if (!found) {
			camTexture = new WebCamTexture ();
		}

		camTexture.requestedHeight = (int)panelRect.height;
		camTexture.requestedWidth = (int)panelRect.width;
		if (camTexture != null) {
			camTexture.Play ();
		}

		//set camera active
		isActive = true;

	}

	void OnGUI () {
		// drawing the camera on screen
		GUI.DrawTexture (cameraRect, camTexture, ScaleMode.ScaleAndCrop);

		// do the reading — you might want to attempt to read less often than you draw on the screen for performance sake
		if (isActive) {
			try {
				IBarcodeReader barcodeReader = new BarcodeReader ();
				// decode the current frame
				Result result = barcodeReader.Decode (camTexture.GetPixels32 (), camTexture.width, camTexture.height);
				if (result != null) {
					ReadQR (result);
				}
			} catch (Exception ex) {
				Debug.LogWarning (ex.Message);
			}
		}
	}


	// actions to do when read QR
	private void ReadQR (Result result)
	{

		int id;
		if (!int.TryParse (result.Text, out id)) {
			id = -1;
			Debug.Log ("VAIR_ QR Code not a integer");
		} else {

			VRUser user;
			if (dbManager.ReadDB (id,out user)) {
				isActive = false;
				ApplicationControl.FoundUserOnDatabase (user);
			}

		}


	}
}
