using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	public Transform cameraTransform;

	// Update is called once per frame
	void Update () {

		transform.rotation = Quaternion.Euler (new Vector3 (0, cameraTransform.rotation.eulerAngles.y, 0));

	}
}
