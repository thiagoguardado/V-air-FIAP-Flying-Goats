using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class airplaneMovement : MonoBehaviour {

	Vector3 target;
	public Text warning;
	public Text remaining_time;
	public static int time = 20;
	public AudioClip loop;
	bool playing_intro = true;


	void Start () {
		StartCoroutine (TimeLeft ());
		StartCoroutine (StartLoop ());
	}

	void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, 60f);
		Movement ();
		remaining_time.text = time.ToString ();
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

		transform.position = Vector3.MoveTowards(transform.position, target, 0.1f);
	}

	void OnTriggerEnter (Collider ring){
		if (ring.tag == "full-score") {
			print ("full");
			ring.gameObject.GetComponent<BoxCollider>().enabled = false;
			ring.transform.parent.gameObject.GetComponent<MeshCollider>().enabled = false;
			ring.gameObject.GetComponent<AudioSource>().Play();
			time = time + 5;
		} else if (ring.tag == "ring") {
			print ("score");
			ring.gameObject.GetComponent<MeshCollider> ().enabled = false;
			ring.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			ring.gameObject.GetComponentInChildren<BoxCollider> ().enabled = false;
			ring.gameObject.GetComponent<AudioSource>().Play();
			time = time + 2;
		}
	}

	IEnumerator TimeLeft (){
		
		yield return new WaitForSeconds (1);
		time--;
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
