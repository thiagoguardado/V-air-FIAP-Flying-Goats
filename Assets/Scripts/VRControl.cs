using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class VRControl : MonoBehaviour {

	public static VRControl instance = null;

	public GameObject pausePanel;
	public GameObject warningPanel;
	private bool isPaused = false;
	private bool hasWarning = false;
	private static bool canResumeWarning = true;
	public string[] pausableScenes;


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


	void Update(){
	
		PauseButtonPress ();

	}


	public static void SetWarning(string title_body_footer, AudioClip audioWarning, bool canResume, float waitingTimeIfNotResumable) {

		string[] warning = title_body_footer.Split ('_');

		if (warning.Length < 3) {
			Debug.LogError ("mensagem de warning incompleta");
			return;
		}

		// setup text and audio
		WarningPanel wp = instance.warningPanel.GetComponent<WarningPanel> ();
		wp.SetupText (warning [0], warning [1], warning [2]);
		wp.PlayWarningAudio (audioWarning);
		if (canResume) {
			canResumeWarning = canResume;
		} else {
			instance.StartCoroutine (instance.FinishSession (waitingTimeIfNotResumable));
		}


		// show panel
		instance.OpenWarningPanel ();

	}


	private void PauseButtonPress ()
	{
		if (Input.GetButtonDown ("Cancel")) {
			for (int i = 0; i < pausableScenes.Length; i++) {
				if (SceneManager.GetActiveScene ().name == pausableScenes [i]) {


					if (hasWarning){
						if (canResumeWarning) {
							ResumeWarningPanel ();
							return;
						}
					} else {
						
						if (!isPaused) {
							OpenPausePanel ();
						} else {
							ResumePausePanel ();
						}
					}
					return;


				}
			}
		}
	}


	public static void BackToMenu(){
	
		Initiate.FadeDefault("VR_Intro");
	
	}


	public void OpenPausePanel(){


		PauseScene ();

		// Open Pause Panel
		pausePanel.SetActive(true);
		isPaused = true;

	}


	public void ResumePausePanel(){

		ResumeScene ();

		// close panel
		StartCoroutine (ClosePanel (pausePanel));
		isPaused = false;

	}



	private void OpenWarningPanel(){

		PauseScene ();

		// Open Pause Panel
		warningPanel.SetActive(true);
		hasWarning = true;

	}


	private void ResumeWarningPanel(){

		ResumeScene ();

		// close panel
		StartCoroutine (ClosePanel (warningPanel));
		hasWarning = false;

	}

	private void PauseScene ()
	{
		// pause video if any
		VideoPlayerControl videoControl = FindObjectOfType<VideoPlayerControl> ();
		if (videoControl != null)
			videoControl.PauseVideo ();
		
		// pause game

		// pause quiz

	}


	private void ResumeScene ()
	{
		// resume video if any
		VideoPlayerControl videoControl = FindObjectOfType<VideoPlayerControl> ();
		if (videoControl != null)
			videoControl.UnPauseVideo ();
		
		// resume game

		// resume quiz

	}


	IEnumerator ClosePanel(GameObject panel){
	
		
		panel.GetComponent<Animator>().SetTrigger("Close");

		yield return new WaitForSeconds (1f);

		panel.SetActive (false);
	
	}


	IEnumerator FinishSession(float waitingTime){


		yield return new WaitForSeconds (waitingTime);

		PlayerDevice.EndSession ();


	}




}
