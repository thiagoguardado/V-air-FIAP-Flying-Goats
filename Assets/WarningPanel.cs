using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningPanel : MonoBehaviour {

	public Text title;
	public Text body;
	public Text footer;

	private AudioSource audioSource;


	public void SetupText(string title, string body, string footer){
	
		this.title.text = title;
		this.body.text = body;
		this.footer.text = footer;
	
	}


	public void PlayWarningAudio(AudioClip clip){
		audioSource.PlayOneShot (clip);
	}

}
