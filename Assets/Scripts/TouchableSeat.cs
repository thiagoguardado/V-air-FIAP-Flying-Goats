using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TouchableSeat : MonoBehaviour {

	public enum SeatStatus {
		Available,
		Occupied,
		Selected
	}

	public SeatStatus seatStatus;
	public string seatName{
		get{ 
			return gameObject.name;
		}
	}

	public Sprite availableSprite;
	public Sprite occupiedSprite;
	public Sprite selectedSprite;

	private SelectSeatControl selectControl;
	private GameObject[] allSeats;
	private Image image;


	void Awake(){
		//get reference
		selectControl = (SelectSeatControl)GameObject.FindObjectOfType<SelectSeatControl> ();
		allSeats = GameObject.FindGameObjectsWithTag("Seat");
		image = GetComponent<Image> ();


		ChangeImage ();

	}


	public void TouchOnSeat(){
	
		Debug.Log ("Touched on seat " + seatName + ". Seat " + seatStatus.ToString());
	
		switch (seatStatus) {
		case SeatStatus.Available:
			SeatAvailable ();
			break;
		case SeatStatus.Occupied:
			SeatOccupied ();
			break;
		case SeatStatus.Selected:
			SeatSelected ();
			break;
		default:
			break;
		}

		ChangeAllImages ();

	}


	void SeatAvailable()
	{

		//deselect all other seats
		DeselectAll ();

		// select this seat
		seatStatus = SeatStatus.Selected;

		// send selection info to control
		selectControl.SeatSelected (seatName);


	}


	void SeatOccupied ()
	{

		//deselect all other seats
		DeselectAll ();

		// deactivate button
		selectControl.SeatDeselected ();

		//send message
		selectControl.SeatTaken (seatName);
	}


	void SeatSelected ()
	{


	}

	void Deselect ()
	{
		seatStatus = SeatStatus.Available;
	}


	void DeselectAll ()
	{
		for (int i = 0; i < allSeats.Length; i++) {
			if (allSeats [i].GetComponent<TouchableSeat> ().seatStatus == SeatStatus.Selected) {
				allSeats [i].GetComponent<TouchableSeat> ().Deselect ();
			}
		}
	}


	public void ChangeAllImages(){
		
		for (int i = 0; i < allSeats.Length; i++) {

			allSeats [i].GetComponent<TouchableSeat> ().ChangeImage ();

		}
	}


	void ChangeImage(){

		Sprite sprite;

		switch (seatStatus) {
		case SeatStatus.Available:
			sprite = availableSprite;
			break;
		case SeatStatus.Occupied:
			sprite = occupiedSprite;
			break;
		case SeatStatus.Selected:
			sprite = selectedSprite;
			break;
		default:
			sprite = availableSprite;
			break;
		}

		image.sprite = sprite;

	}
}
