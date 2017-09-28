using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.UI;

public class NewIntroManager : MonoBehaviour {

	public Text nome;

	public GameObject firstpanel;
	public GameObject secondPanel;
	public GameObject button;

	bool firstbutton = false;
	public float timeToButton = 9f;

	[SerializeField] private Reticle m_Reticle;                         // The scene only uses SelectionSliders so the reticle should be shown.
	[SerializeField] private SelectionRadial m_Radial;                  // Likewise, since only SelectionSliders are used, the radial should be hidden.  
	[SerializeField] private UIFader m_HowToUseConfirmFader;            // Afterwards users are asked to confirm how to use sliders in this UI.
	[SerializeField] private SelectionSlider m_HowToUseConfirmSlider;   // They demonstrate this using this slider.


	void Awake(){

		nome.text = PlayerDevice.currentUser.name.ToString ();

	}

	// Update is called once per frame
	void Update () {

		if (!firstbutton) {
			if (Input.GetButtonDown ("Fire1")) {
			
				ChangeFirst ();
				firstbutton = true;


				StartCoroutine (ButtonActive());

			}
		
		}


	}



	void ChangeFirst(){
	
		firstpanel.SetActive (false);
		secondPanel.SetActive (true);



	}

	IEnumerator ButtonActive(){
	
		yield return new WaitForSeconds (timeToButton);

		button.SetActive (true);

		m_Reticle.Show ();

		m_Radial.Hide ();


		m_HowToUseConfirmSlider.OnBarFilled += LoadMenu;

		yield return StartCoroutine(m_HowToUseConfirmFader.InteruptAndFadeIn());
		yield return StartCoroutine(m_HowToUseConfirmSlider.WaitForBarToFill());
		yield return StartCoroutine(m_HowToUseConfirmFader.InteruptAndFadeOut());

	
	}


	public void LoadMenu(){

		Initiate.FadeDefault ("VR_MainMenu");
	
		Debug.Break ();

	}

}

