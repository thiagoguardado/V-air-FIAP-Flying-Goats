using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingForUserScreen : MonoBehaviour {

	public Text greetings;
	public Text timer;
	public int timerDuration;
	private int timerCount = 0;


	void Awake(){

		greetings.text = "Aguardando usuário " + PlayerDevice.currentUser.name;
		timerCount = timerDuration;

		StartCoroutine (Countdown ());
	}
		

	void Update(){

		// recognize player input here to start application

	}

	IEnumerator Countdown(){

		timer.text = timerCount.ToString ();

		while (timerCount >= 0) {

			yield return new WaitForSeconds (1f);
			timerCount -= 1;
			timer.text = timerCount.ToString ();
		
		}
	
		BackToIdle ();

	}

	void BackToIdle ()
	{
		PlayerDevice.EndSession ();
		SceneManager.LoadScene ("VR_Idle");
	}
}
