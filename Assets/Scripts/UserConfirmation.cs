using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserConfirmation : MonoBehaviour {

	public Text nome;
	public Text voo;
	public Text localizador;
	public Text hora;


	void Awake(){

		FillTexts ();

	}

	public void FillTexts(){

		nome.text = ApplicationControl.currentSelectingUser.name.ToString ();
		voo.text = ApplicationControl.currentSelectingUser.flight.ToString ();
		localizador.text = ApplicationControl.currentSelectingUser.locator.ToString ();
		hora.text = ApplicationControl.currentSelectingUser.flightTime.ToString ();


	}


	public void Confirm(){

		Initiate.FadeDefault ("Totem_ChooseYourSeat");

	}


	public void Cancel(){
	
		ApplicationControl.BackToScanMenu ();
	
	}

}
