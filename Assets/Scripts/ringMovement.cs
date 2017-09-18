using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ringMovement : MonoBehaviour {

	int speed = 30;

	void Update () {
		transform.Translate (Vector3.back * Time.deltaTime * speed);
		if (transform.position.z < 0f) {
			Destroy (this.gameObject);
		}
	}

}
