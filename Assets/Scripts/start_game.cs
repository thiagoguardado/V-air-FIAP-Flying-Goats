using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_game : MonoBehaviour {

	float speed, up_speed;
	bool do_animation = false;
	public GameObject plane, instantiator, cloud_instantiator;
	public GameObject start_menu, instructions, menu, end_menu;
	public GameObject pause_canvas;
	GameObject[] rings, clouds;
	RaycastHit hit;
	Vector3 target;
	bool checking = false;
	bool paused, in_end_menu = false;
	bool is_playing;
	public AudioClip chose, confirm, back;
	AudioSource audio_source;
	public Texture play_selected;
	public Texture exit_selected;
	public Texture quit_selected;
	public GameObject last_game_object;
	public Texture play_unselected;
	public Texture exit_unselected;
	public Texture quit_unselected;
	public GameObject bar;
	RectTransform barRect;
	public RectTransform select_bar;
	public GameObject camera;



	void Awake () {
		speed = 0f;
		up_speed = 0f;
		audio_source = gameObject.GetComponent<AudioSource> ();
		barRect = bar.GetComponent<RectTransform> ();
	}


	void Update () {
		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));

		if (Physics.Raycast (ray, out hit)) {
			Vector3 target = new Vector3 (hit.point.x, hit.point.y, transform.position.z);
			if (hit.transform.gameObject.layer == 14 && !checking) {
				checking = true;
				StartCoroutine (Verifier ());
			}
			if (!in_end_menu) {
				if (hit.transform.tag == "start") {
					if (barRect.transform.position.x >= -1f) {
						barRect.transform.Translate (Vector3.left);
					} else {
						barRect.transform.position = new Vector3 (-1.6f, 0.02f, 5.5f);
					}
					last_game_object = hit.transform.gameObject;
					hit.transform.gameObject.GetComponent<Renderer> ().material.mainTexture = play_selected;
				} else if (hit.transform.tag == "play") {
					last_game_object = hit.transform.gameObject;
					hit.transform.gameObject.GetComponent<Renderer> ().material.mainTexture = play_selected;
				} else if (hit.transform.tag == "exit") {
					if (barRect.transform.position.x <= 1f) {
						barRect.transform.Translate (Vector3.right);
					} else {
						barRect.transform.position = new Vector3 (1.6f, 0.02f, 5.5f);
					}
					last_game_object = hit.transform.gameObject;
					hit.transform.gameObject.GetComponent<Renderer> ().material.mainTexture = exit_selected;
				} else if (hit.transform.tag == "quit") {
					last_game_object = hit.transform.gameObject;
					hit.transform.gameObject.GetComponent<Renderer> ().material.mainTexture = quit_selected;
				} else {
					if (last_game_object != null) {
						if (last_game_object.tag == "start") {
							last_game_object.GetComponent<Renderer> ().material.mainTexture = play_unselected;
						} else if (last_game_object.tag == "play") {
							last_game_object.GetComponent<Renderer> ().material.mainTexture = play_unselected;
						} else if (last_game_object.tag == "exit") {
							last_game_object.GetComponent<Renderer> ().material.mainTexture = exit_unselected;
						} else if (last_game_object.tag == "quit") {
							last_game_object.GetComponent<Renderer> ().material.mainTexture = quit_unselected;
						}
					}
				}
			}
		}
		if (do_animation) {
			plane.GetComponent<AudioSource> ().enabled = true;
			is_playing = true;
			speed = speed + 5.3f;
			if (speed > 50f) {
				transform.Translate (Vector3.down * Time.deltaTime * up_speed);
				up_speed = up_speed + 0.01f;
			}
			transform.Translate (Vector3.back * Time.deltaTime * speed);
			if (transform.position.y < -18) {
				GetComponent<MeshRenderer> ().enabled = false;
			} 
		}

//		if (Input.GetButtonDown("Fire1") && is_playing) {
//			PauseGame ();
//		}

		if (Input.GetButtonDown("Fire1") && paused || Input.GetButtonDown("Fire1") && in_end_menu) {
			checking = true;
			StartCoroutine(Verifier ());
		}

		if (airplaneMovement.minutes < 0 && !in_end_menu) {
			in_end_menu = true;
			StartCoroutine(FinishGame (0));
		} else if (airplaneMovement.score >= 250 && !in_end_menu) {
			in_end_menu = true;
			StartCoroutine(FinishGame (1));
		}
	}

	IEnumerator EndAnimation(){
		yield return new WaitForSeconds (2f);
		while (transform.position.y > -36f) {
			do_animation = true;
			yield return null;
		}
		do_animation = false;
		instantiator.GetComponent<ringInstantiator> ().enabled = true;
		plane.GetComponent<airplaneMovement> ().enabled = true;
		cloud_instantiator.GetComponent<CloudInstantiator> ().enabled = true;

	}

	IEnumerator Verifier (){
		int time = 0;
		while (checking) {
			time = time + 1;
			yield return null;
			if (hit.transform.tag == "start" && Input.GetButton("Fire1")) {
				select_bar.localScale = new Vector3 (time * 0.02f, 1f, 1f);
				if (time > 50) {
					checking = false;
					audio_source.clip = chose;
					audio_source.Play ();
					last_game_object = null;
					barRect.transform.position = new Vector3 (0, -0.60f, 5.5f);
					Destroy (start_menu.gameObject);
				}
			} else if (hit.transform.tag == "play" && Input.GetButton("Fire1")) {
				select_bar.localScale = new Vector3 (time * 0.02f, 1f, 1f);
				if (time > 50) {
					audio_source.clip = confirm;
					audio_source.Play ();
					last_game_object = null;
					bar.GetComponent<Canvas> ().enabled = false;
					Destroy (instructions.gameObject);
					StartCoroutine (EndAnimation ());
				}
			} else if (hit.transform.tag == "exit" && Input.GetButton("Fire1")) {
				select_bar.localScale = new Vector3 (time * 0.02f, 1f, 1f);
				if (time > 50) {
					checking = false;
					Initiate.FadeDefault ("VR_MainMenu");
//					Application.LoadLevel ("VR_MainMenu");
				}
			} else if (hit.transform.tag == "quit" && Input.GetButton("Fire1") && paused) {
				select_bar.localScale = new Vector3 (time * 0.02f, 1f, 1f);
				if (time > 50) {
					Application.Quit ();
				}
			} else {
				checking = false;
			}

		}
		time = 0;
		select_bar.localScale = new Vector3 (0, 1f, 1f);
	}

	public void PauseGame(){

		if (paused) {
//		if (hit.transform.tag == "pause_menu") {
			Time.timeScale = 1;
			audio_source.clip = chose;
			audio_source.Play ();
			plane.GetComponent<AudioSource> ().volume = 0.3f;
//			menu.GetComponent<MeshRenderer> ().enabled = false;
//			menu.GetComponent<MeshCollider> ().enabled = false;
//			menu.transform.GetChild (0).gameObject.GetComponent<MeshRenderer> ().enabled = false;
//			menu.transform.GetChild (0).gameObject.GetComponent<MeshCollider> ().enabled = false;
//			menu.transform.GetChild (1).gameObject.GetComponent<MeshRenderer> ().enabled = false;
//			menu.transform.GetChild (1).gameObject.GetComponent<MeshCollider> ().enabled = false;
//			menu.transform.GetChild (2).gameObject.GetComponent<Canvas> ().enabled = false;
			paused = false;
		} else {
			Time.timeScale = 0;
			audio_source.clip = back;
			audio_source.Play ();
//			menu.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
//			menu.transform.GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = true;
//			menu.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = true;
//			menu.transform.GetChild(1).gameObject.GetComponent<MeshCollider>().enabled = true;
//			menu.transform.GetChild(2).gameObject.GetComponent<Canvas>().enabled = true;
			plane.GetComponent<AudioSource> ().volume = 0f;
//			menu.GetComponent<MeshRenderer> ().enabled = true;
//			menu.GetComponent<MeshCollider> ().enabled = true;
			paused = true;
		}
	}

	IEnumerator FinishGame(int condition){
		is_playing = false;
		plane.GetComponent<airplaneMovement> ().enabled = false;
		plane.GetComponent<airplaneMovement> ().remaining_time.enabled = false;
		plane.GetComponent<airplaneMovement> ().actual_score.enabled = false;
		int count = 0;
		while (count < 100) {
			yield return null;
			count++;
			plane.transform.Translate (Vector3.forward * Time.deltaTime * 150);
		}
		FinishGame (condition);
		Destroy (instantiator.gameObject);
		Destroy (cloud_instantiator.gameObject);
		rings = GameObject.FindGameObjectsWithTag ("ring");
		clouds = GameObject.FindGameObjectsWithTag ("cloud");
		foreach (GameObject ring in rings) {
			Destroy (ring.gameObject);
		}
		foreach (GameObject cloud in clouds) {
			Destroy (cloud.gameObject);
		}
		end_menu.GetComponent<MeshCollider> ().enabled = true;
		end_menu.GetComponent<MeshRenderer> ().enabled = true;
		end_menu.transform.GetChild (condition).gameObject.GetComponent<Canvas> ().enabled = true;
		plane.GetComponent<AudioSource> ().volume = 0f;
		plane.GetComponent<airplaneMovement> ().warning.enabled = false;

		bar.GetComponent<Canvas> ().enabled = true;
		barRect.localScale = new Vector3 (0.17f, 0.15f, 0.3f);
		barRect.transform.position = new Vector3 (0, -0.15f, 0.5f);
		yield return null;
	}
}
