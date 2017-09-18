using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class airplaneMovement : MonoBehaviour {

	//public Camera camera;
	Vector3 target;
	public Text warning;
	public Text remaining_time;
	public int time = 20;

	// Use this for initialization
	void Start () {
		StartCoroutine (TimeLeft ());
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();

		remaining_time.text = time.ToString();
	}

	void Movement () {
		
		RaycastHit hit;
		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));

		if (Physics.Raycast (ray, out hit)) {
			if (hit.transform.tag == "playable") {
				target = new Vector3 (hit.point.x, hit.point.y, transform.position.z);
				//transform.LookAt (target);
				warning.GetComponent<Text> ().enabled = false;
			} else if (hit.transform.tag == "warning") {
				warning.GetComponent<Text> ().enabled = true;
			}
		}

		transform.position = Vector3.MoveTowards(transform.position, target, 0.05f);
	}

	void OnTriggerEnter (Collider ring){

		if (ring.tag == "full-score") {
			print ("full");
			time = time + 10;
			Destroy (ring.gameObject);
		} else if (ring.tag == "ring") {
			print ("ring");
			time = time + 5;
			Destroy (ring.gameObject);
		}
	}

	IEnumerator TimeLeft (){
		
		yield return new WaitForSeconds (1);
		time--;

		StartCoroutine (TimeLeft ());
	}
}
