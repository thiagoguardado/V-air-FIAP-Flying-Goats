using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloud : MonoBehaviour {

	float speed = 40;

	void Update () {
		transform.Translate(Vector3.back * speed * Time.deltaTime);
		if (transform.position.z < -20f) {
			Destroy (this.gameObject);
		}
	}
}
