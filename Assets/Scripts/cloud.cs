using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

	float speed = 40;

	void Update () {
		transform.Translate(Vector3.back * speed * Time.deltaTime);
	}
}
