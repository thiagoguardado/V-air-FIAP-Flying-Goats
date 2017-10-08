using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class VRControl : MonoBehaviour {

	public static VRControl instance = null;

	public Canvas canvas;

	public GameObject pausePanel;
	public Button continuarBotao;

	public GameObject warningPanel;
	private bool isPaused = false;
	private bool isClosing = false;
	private bool hasWarning = false;
	private static bool canResumeWarning = true;

	public string[] returnableScenes;
	public string[] pausableScenes;

	public float lookingDownMaxAngle = 50;
	private bool lookingDown = false;
	private float lookingDownTimer = 0f;
	public float timeLookingDown=2f;

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

		LookingDown ();


		AlignToCamera ();

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
		wp.PlayWarningAudio ();
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


					PauseAction ();
					return;

				}
			}
		}
	}


	void LookingDown ()
	{

		if (!isPaused) {

			float angle = Vector3.Angle (Vector3.down, Camera.main.transform.forward);
			if (!lookingDown) {
				if (angle <= lookingDownMaxAngle) {
					lookingDown = true;
					lookingDownTimer = 0f;
				}
			} else {

				if (angle > lookingDownMaxAngle) {

					lookingDown = false;
					return;
				}

				lookingDownTimer += Time.deltaTime;

				if (lookingDownTimer >= timeLookingDown) {
			
					for (int i = 0; i < pausableScenes.Length; i++) {

						if (SceneManager.GetActiveScene ().name == pausableScenes [i]) {

							OpenPausePanel ();
							return;

						}

					}

					for (int i = 0; i < returnableScenes.Length; i++) {

						if (SceneManager.GetActiveScene ().name == returnableScenes [i]) {

							VRControl.BackToMenu ();
							return;

						}

					}

					lookingDownTimer = 0f;
			
				}
			}
		}


	}

	void AlignToCamera ()
	{
		transform.rotation = Camera.main.transform.rotation;
	}

	void PauseAction ()
	{
		if (!isClosing) {

			if (hasWarning) {
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


	public static void BackToMenu(){
	
		instance.warningPanel.SetActive (false);
		instance.pausePanel.SetActive (false);

		Initiate.FadeDefault("VR_MainMenu");
	
	}


	public void OpenPausePanel(){

		transform.parent = Camera.main.transform;
		canvas.worldCamera = Camera.main;
		transform.localPosition = new Vector3 (0, 0, 1);

		PauseScene ();

		// Open Pause Panel
		pausePanel.SetActive(true);
		pausePanel.GetComponent<Animator> ().SetTrigger ("Open");
		isPaused = true;

		EventSystem.current.SetSelectedGameObject (null);
		EventSystem.current.SetSelectedGameObject(continuarBotao.gameObject); 

	}


	public void ResumePausePanel(){

		ResumeScene ();

		// close panel
		StartCoroutine (ClosePanel (pausePanel));
		isPaused = false;

	}



	private void OpenWarningPanel(){

		transform.parent = Camera.main.transform;
		canvas.worldCamera = Camera.main;
		transform.localPosition = new Vector3 (0, 0, 1);

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
		start_game startGame = FindObjectOfType<start_game> ();
		if (startGame != null)
			startGame.PauseGame ();
		
		// pause quiz

	}


	private void ResumeScene ()
	{
		// resume video if any
		VideoPlayerControl videoControl = FindObjectOfType<VideoPlayerControl> ();
		if (videoControl != null)
			videoControl.UnPauseVideo ();
		
		// resume game
		// pause game
		start_game startGame = FindObjectOfType<start_game> ();
		if (startGame != null)
			startGame.PauseGame ();


		// resume quiz

	}


	IEnumerator ClosePanel(GameObject panel){
	
		panel.GetComponent<Animator>().SetTrigger("Close");

		isClosing = true;

		yield return new WaitForSeconds (1f);

		panel.SetActive (false);

		isClosing = false;;
	
	}


	IEnumerator FinishSession(float waitingTime){


		yield return new WaitForSeconds (waitingTime);

		PlayerDevice.EndSession ();


	}



	public void BotaoContinuar(){
	
		PauseAction ();
	
	}



	public void BotaoEncerrar(){
	
		pausePanel.SetActive (false);
		warningPanel.SetActive (false);

		Time.timeScale = 1;

		PlayerDevice.EndSession();

	}


	public void BotaoMenu(){

		Time.timeScale = 1;
		BackToMenu ();

	}

}
