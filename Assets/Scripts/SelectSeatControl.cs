using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectSeatControl : MonoBehaviour {

	public GraphicRaycaster raycaster;
	private PointerEventData ped;
	public Text messageOutput;
	public Text greetings;
	public Button confirmButton;
	public string selectedSeat = "";

	void Awake () {

		ped = new PointerEventData (null);

		//initial setup
		greetings.text = "Selecione sua poltrona, " + ApplicationControl.currentSelectingUser.name;
		messageOutput.text = "";
		confirmButton.enabled = false;

		//get all seats status



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


		// display message
		messageOutput.text = "Assento " + selectedSeat + " liberado!";

	}
}
