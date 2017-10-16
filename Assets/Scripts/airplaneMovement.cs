using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class airplaneMovement : MonoBehaviour {

	Vector3 target;
	public Text warning;
	public Text remaining_time, actual_score;
	public static int minutes = 3, score = 0;
	int seconds = 0;
	string zero;
	public AudioClip loop;
	bool playing_intro = true;
	public GameObject aim;
	public float speed;
	float dif;
	float turn_at;
	bool turn_verify = true;
	public ParticleSystem left_w, right_w;

	void Start () {
		left_w.Play ();
		right_w.Play ();
		StartCoroutine (TimeLeft ());
		StartCoroutine (StartLoop ());
	}

	void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, 60f);
		Movement ();
		if (seconds < 10) {
			zero = "0";
		} else {
			zero = "";
		}
		remaining_time.text = minutes.ToString () + ":" + zero + seconds.ToString();
		actual_score.text = score.ToString () + "/250";
//		dif = aim.transform.position.y - transform.position.y;
//		if (dif < 6 && dif > -6) {
//			aim.transform.position = new Vector3 (transform.position.x, aim.transform.position.y, aim.transform.position.z);
//			last_position = aim.transform.position;
//		} else {
//			aim.transform.position = last_position;
//		}
	}

	void Movement () {
		
		RaycastHit hit;
		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));

		if (Physics.Raycast (ray, out hit)) {
			if (hit.transform.tag == "playable") {
				target = new Vector3 (hit.point.x, hit.point.y, transform.position.z);
				warning.GetComponent<Text> ().enabled = false;
			} else if (hit.transform.tag == "warning") {
				warning.GetComponent<Text> ().enabled = true;
			}
		}


		//Tentativa 1 de motivmento, fez o avião parecer um peixe fora d'água
//		if (transform.rotation.x >= -0.0050f && transform.rotation.x <= 0.0050f && transform.rotation.y >= -0.0050f && transform.rotation.y <= 0.0050f) {
//			print (transform.rotation.x);
//			print ("what?");
//			transform.LookAt (aim.transform);
//		} else {
//			transform.LookAt (new Vector3 (transform.position.x, transform.position.y, 80f));
//		}
		//transform.LookAt (new Vector3(transform.position.x, aim.transform.position.y, 80f));

		dif = aim.transform.position.x - transform.rotation.z;
		if (dif > 25) {
			if (turn_verify) {
				StartCoroutine(TurnPlane (-15, 0.5f));
			}
		} else if (dif < -25){
			if (turn_verify) {
				StartCoroutine(TurnPlane (15, 0.5f));
			}
		} else {
			if (turn_at > 0) {
				turn_at = turn_at - 0.5f;
			} else if (turn_at < 0){
				turn_at = turn_at + 0.5f;
			}
		}

		transform.rotation = Quaternion.Euler (-0.5f * aim.transform.position.y, transform.rotation.y, turn_at);
		transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
	}

	IEnumerator TurnPlane(int condition, float turn){
		turn_verify = false;
		while (turn_at != condition) {
			yield return null;
			if (condition < 0) {
				turn_at = turn_at - turn;
			} else {
				turn_at = turn_at + turn;
			}
		}
		turn_verify = true;
	}

	void OnTriggerEnter (Collider ring){
		if (ring.tag == "full-score") {
			ring.gameObject.GetComponent<BoxCollider>().enabled = false;
			ring.transform.parent.gameObject.GetComponent<MeshCollider>().enabled = false;
			ring.gameObject.GetComponent<AudioSource>().Play();
			ring.transform.parent.GetComponent<ParticleSystem>().Play ();
			score = score + 10;
		} else if (ring.tag == "ring") {
			ring.gameObject.GetComponent<MeshCollider> ().enabled = false;
			ring.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			ring.gameObject.GetComponentInChildren<BoxCollider> ().enabled = false;
			ring.gameObject.GetComponent<AudioSource>().Play();
			ring.GetComponent<ParticleSystem>().Play ();
			score = score + 5;
		}
	}

	IEnumerator TimeLeft (){
		yield return new WaitForSeconds (1);
		if (seconds - 1 < 0) {
			minutes--;
			seconds = 59;
		} else {
			seconds--;
		}
		StartCoroutine (TimeLeft ());
	}

	IEnumerator StartLoop (){
		yield return null;
		if (gameObject.GetComponent<AudioSource> ().isPlaying == false) {
			gameObject.GetComponent<AudioSource> ().clip = loop;
			gameObject.GetComponent<AudioSource> ().Play ();
			gameObject.GetComponent<AudioSource> ().loop = true;
		} else {
			StartCoroutine (StartLoop ());
		}
	}
}
