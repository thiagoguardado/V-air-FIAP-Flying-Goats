using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class SelectSeatControl : MonoBehaviour {

	public GraphicRaycaster raycaster;
	private PointerEventData ped;
	public Text messageOutput;
	public Button confirmButton;
	public string selectedSeat = "";
	public GameObject painelConfirmacao;
	public Text textoPainelConfirmacao;
	public Text poltronaPainelConfirmacao;
	public Text timerConfirmacao;
	public int timerDuration = 20;
	public bool refreshStatus = false;
	public float refreshInterval = 1;



	void Awake () {

		ped = new PointerEventData (null);

		//initial setup
		messageOutput.text = "Selecione sua poltrona, " + ApplicationControl.currentSelectingUser.name;
		messageOutput.text = "";
		confirmButton.enabled = false;
		painelConfirmacao.SetActive (false);


		//get all seats status
		if (refreshStatus) {
			StartCoroutine (RefreshSeatStatus ());
		}
	

	}
	
	// Update is called once per frame
	void Update () {

		// look for touch or mouse click
		if (Input.touchCount > 0) {
			RaycastCanvas (Input.GetTouch (0).position);
		} else {
			if (Input.GetMouseButtonDown (0)) {
				RaycastCanvas (Input.mousePosition);
			}
		}


	}

	void RaycastCanvas (Vector2 pointerPosition)
	{
		List<RaycastResult> results = new List<RaycastResult> ();
		ped.position = pointerPosition;
		raycaster.Raycast (ped, results);
		if (results.Count > 0) {
			TouchableSeat tseat = results [0].gameObject.GetComponent<TouchableSeat> ();
			if (tseat != null) {
				tseat.TouchOnSeat ();
			}
		}
	}


	public void SeatSelected (string seatName)
	{
		messageOutput.text = "Confirme a seleção do assento " + seatName + "!";
		selectedSeat = seatName;
		confirmButton.enabled = true;
	}

	public void SeatDeselected(){
		selectedSeat = "";
		confirmButton.enabled = false;
	}

	public void SeatTaken(string seatName){
		messageOutput.text = "Assento " + seatName + " não disponível. Selecione outro!";
	} 


	public void ConfirmSeatSelection(){

		// send message to VR application to start session
		SendInfoToVR.FindDevice(selectedSeat,ApplicationControl.currentSelectingUser.id.ToString());


		// wait for device to send back a signal


		// display panel
		StartConfirmation();

	}


	private void StartConfirmation(){
	
		// ativa painel
		painelConfirmacao.SetActive (true);
		textoPainelConfirmacao.text = ApplicationControl.currentSelectingUser.name + " dirija-se à poltrona:";
		poltronaPainelConfirmacao.text = selectedSeat;

		// ativa timer
		StartCoroutine(Timer());


	}

	IEnumerator RefreshSeatStatus ()
	{

		SendInfoToVR.PingAllDevices ();

		yield return new WaitForSeconds (refreshInterval);

		TouchableSeat[] seats = FindObjectsOfType<TouchableSeat> ();
		DeviceInfo[] devices = TotemManager.devices;
		for (int i = 0; i < devices.Length; i++) {
			for (int j = 0; j < seats.Length; j++) {
				if (devices [i].deviceID == seats [j].seatName) {
					if (!(devices [i].deviceStatus == DeviceStatus.idle)) {
						seats [j].seatStatus = TouchableSeat.SeatStatus.Occupied;
						break;
					} 
				}
			}
		}

		seats [0].ChangeAllImages ();

		StartCoroutine (RefreshSeatStatus ());

	}

	IEnumerator Timer(){

		int timercont = timerDuration;

		timerConfirmacao.text = timerDuration.ToString ();
		
		while (timercont >= 0) {

			yield return new WaitForSeconds (1f);
			timerConfirmacao.text = timercont.ToString ();
			timercont -= 1;

		}

		BackToScan ();

	}

	public void BackToScan(){

		ApplicationControl.BackToScanMenu ();

	}

}
