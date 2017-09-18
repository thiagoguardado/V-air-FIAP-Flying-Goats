using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_game : MonoBehaviour {

	float speed, up_speed;
	bool do_animation = false;
	public GameObject plane, instantiator;
	public GameObject start_menu, instructions, menu, lose_menu;
	RaycastHit hit;
	Vector3 target;
	bool checking = false;
	bool paused, in_lose_menu;
	bool is_playing;
	public AudioClip chose, confirm, back;
	AudioSource audio_source;

	void Start () {
		speed = 0f;
		up_speed = 0f;
		audio_source = gameObject.GetComponent<AudioSource> ();
	}
	

	void Update () {
		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));

		if (Physics.Raycast (ray, out hit)) {
			Vector3 target = new Vector3 (hit.point.x, hit.point.y, transform.position.z);
			if (hit.transform.gameObject.layer == 14 && !checking) {
				checking = true;
				StartCoroutine (Verifier ());
			}
		}
		if (do_animation) {
			plane.GetComponent<AudioSource> ().enabled = true;
			is_playing = true;
			speed = speed + 0.3f;
			if (speed > 50f) {
				transform.Translate (Vector3.down * Time.deltaTime * up_speed);
				up_speed = up_speed + 0.01f;
			}
			transform.Translate (Vector3.back * Time.deltaTime * speed);
		}

		if (Input.GetButtonDown("Fire1") && is_playing) {
			PauseGame ();
		}

		if (Input.GetButtonDown("Fire1") && paused || Input.GetButtonDown("Fire1") && in_lose_menu) {
			checking = true;
			StartCoroutine(Verifier ());
		}

		if (airplaneMovement.time <= 0) {
			in_lose_menu = true;
			LoseGame();
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

	}

	IEnumerator Verifier (){
		int time = 0;
		while (checking) {
			time = time + 1;

			yield return null;
			if (hit.transform.tag == "start" && Input.GetButton("Fire1")) {
				if (time > 150) {
					checking = false;
					audio_source.clip = chose;
					audio_source.Play ();
					Destroy (start_menu.gameObject);
				}
			} else if (hit.transform.tag == "play" && Input.GetButton("Fire1")) {
				if (time > 150) {
					audio_source.clip = confirm;
					audio_source.Play ();
					Destroy (instructions.gameObject);
					StartCoroutine (EndAnimation ());
				}
			} else if (hit.transform.tag == "exit" && Input.GetButton("Fire1")) {
				if (time > 150) {
					Application.LoadLevel ("VR_Intro");
				}
			} else if (hit.transform.tag == "exit" && Input.GetButton("Fire1") && paused) {
				if (time > 150) {
					Application.LoadLevel ("VR_Intro");
				}
			} else {
				checking = false;
			}

		}
		time = 0;
	}

	void PauseGame(){
		if (hit.transform.tag == "pause_menu") {
			Time.timeScale = 1;
			audio_source.clip = chose;
			audio_source.Play ();
			plane.GetComponent<AudioSource> ().volume = 0.3f;
			menu.GetComponent<MeshRenderer> ().enabled = false;
			menu.GetComponent<MeshCollider> ().enabled = false;
			menu.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
			menu.transform.GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = false;
			paused = false;
		} else {
			Time.timeScale = 0;
			audio_source.clip = back;
			audio_source.Play ();
			menu.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
			menu.transform.GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = true;
			plane.GetComponent<AudioSource> ().volume = 0f;
			menu.GetComponent<MeshRenderer> ().enabled = true;
			menu.GetComponent<MeshCollider> ().enabled = true;
			paused = true;
		}
	}

	void LoseGame(){
		is_playing = false;
		plane.GetComponent<airplaneMovement> ().enabled = false;
		instantiator.GetComponent<ringInstantiator> ().enabled = false;
		lose_menu.GetComponent<MeshCollider> ().enabled = true;
		lose_menu.GetComponent<MeshRenderer> ().enabled = true;
		plane.GetComponent<AudioSource> ().volume = 0f;
		plane.GetComponent<airplaneMovement> ().remaining_time.enabled = false;
		plane.GetComponent<airplaneMovement> ().warning.enabled = false;

	}
}
